using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NewsNet.Application.Abstractions.Authentication;
using NewsNet.Application.Common.Models;
using NewsNet.Domain.Abstractions;
using NewsNet.Domain.Entities;
using NewsNet.Infrastructure.Persistence;
using System.Security.Cryptography;

namespace NewsNet.Infrastructure.Authentication;

internal sealed class AuthenticationService : IAuthenticationService
{
    private readonly NewsNetDbContext _dbContext;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IClock _clock;
    private readonly JwtOptions _jwtOptions;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthenticationService(
        NewsNetDbContext dbContext,
        IJwtTokenGenerator jwtTokenGenerator,
        IClock clock,
        IOptions<JwtOptions> jwtOptions,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _jwtTokenGenerator = jwtTokenGenerator;
        _clock = clock;
        _jwtOptions = jwtOptions.Value;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<AuthenticationResult?> SignInAsync(string userNameOrEmail, string password, string? ipAddress, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userNameOrEmail) || string.IsNullOrWhiteSpace(password))
            return null;

        var user = await FindUserAsync(userNameOrEmail, cancellationToken);

        if (user is null || !user.IsActive)
            return null;

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);

        if (!signInResult.Succeeded)
            return null;

        var (refreshToken, refreshTokenValue) = await CreateRefreshTokenAsync(user.Id, ipAddress, cancellationToken);
        var (accessToken, accessTokenExpiresAt) = await _jwtTokenGenerator.GenerateAsync(user, cancellationToken);
        var roles = await _userManager.GetRolesAsync(user);

        await PersistRefreshTokenAsync(user.Id, refreshToken, ipAddress, cancellationToken);

        return new AuthenticationResult(
            accessToken,
            accessTokenExpiresAt,
            refreshTokenValue,
            refreshToken.ExpiresAt,
            user.Id,
            user.UserName ?? user.Email ?? user.Id.ToString(),
            user.Email,
            roles.ToArray());
    }

    public async Task<AuthenticationResult?> RefreshTokenAsync(string refreshToken, string? ipAddress, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return null;

        var tokenHash = TokenHasher.Hash(refreshToken);

        var storedToken = await _dbContext.RefreshTokens
            .Include(token => token.User)
            .FirstOrDefaultAsync(token => token.TokenHash == tokenHash, cancellationToken);

        if (storedToken is null)
            return null;

        var now = _clock.UtcNow;

        if (storedToken.RevokedAt is not null || storedToken.ExpiresAt <= now)
            return null;

        if (!storedToken.User.IsActive)
            return null;

        var user = storedToken.User;
        var (newRefreshToken, newRefreshTokenValue) = await CreateRefreshTokenAsync(user.Id, ipAddress, cancellationToken);
        var (accessToken, accessTokenExpiresAt) = await _jwtTokenGenerator.GenerateAsync(user, cancellationToken);
        var roles = await _userManager.GetRolesAsync(user);

        await _dbContext.ExecuteInTransactionAsync(async ct =>
        {
            var refreshedAt = _clock.UtcNow;
            storedToken.Revoke(refreshedAt, ipAddress, "Replaced by refresh token", newRefreshToken.TokenHash);
            _dbContext.RefreshTokens.Add(newRefreshToken);
        }, cancellationToken);

        return new AuthenticationResult(
            accessToken,
            accessTokenExpiresAt,
            newRefreshTokenValue,
            newRefreshToken.ExpiresAt,
            user.Id,
            user.UserName ?? user.Email ?? user.Id.ToString(),
            user.Email,
            roles.ToArray());
    }

    private async Task<ApplicationUser?> FindUserAsync(string userNameOrEmail, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(userNameOrEmail);

        if (user is not null)
            return user;

        if (userNameOrEmail.Contains('@', StringComparison.Ordinal))
            return await _userManager.FindByEmailAsync(userNameOrEmail);

        return await _userManager.Users.FirstOrDefaultAsync(u => u.Email == userNameOrEmail, cancellationToken);
    }

    private async Task PersistRefreshTokenAsync(Guid userId, RefreshToken refreshToken, string? ipAddress, CancellationToken cancellationToken)
    {
        await _dbContext.ExecuteInTransactionAsync(async ct =>
        {
            var now = _clock.UtcNow;
            var activeTokens = await _dbContext.RefreshTokens
                .Where(token => token.UserId == userId && token.RevokedAt == null && token.ExpiresAt > now)
                .ToListAsync(ct);

            foreach (var token in activeTokens)
            {
                token.Revoke(now, ipAddress, "Replaced by new sign-in", refreshToken.TokenHash);
            }

            _dbContext.RefreshTokens.Add(refreshToken);
        }, cancellationToken);
    }

    private async Task<(RefreshToken Token, string PlainText)> CreateRefreshTokenAsync(Guid userId, string? ipAddress, CancellationToken cancellationToken)
    {
        while (true)
        {
            var tokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var tokenHash = TokenHasher.Hash(tokenValue);

            var exists = await _dbContext.RefreshTokens
                .AnyAsync(token => token.TokenHash == tokenHash, cancellationToken);

            if (exists)
                continue;

            var createdAt = _clock.UtcNow;
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TokenHash = tokenHash,
                CreatedAt = createdAt,
                ExpiresAt = createdAt.AddDays(_jwtOptions.RefreshTokenLifetimeDays),
                CreatedByIp = ipAddress
            };

            return (refreshToken, tokenValue);
        }
    }
}
