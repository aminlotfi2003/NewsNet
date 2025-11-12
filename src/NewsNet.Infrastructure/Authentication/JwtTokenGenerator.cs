using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewsNet.Application.Abstractions.Authentication;
using NewsNet.Domain.Abstractions;
using NewsNet.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewsNet.Infrastructure.Authentication;

internal sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly IClock _clock;
    private readonly JwtOptions _options;
    private readonly UserManager<ApplicationUser> _userManager;

    public JwtTokenGenerator(
        IOptions<JwtOptions> options,
        UserManager<ApplicationUser> userManager,
        IClock clock)
    {
        _options = options.Value;
        _userManager = userManager;
        _clock = clock;
    }

    public async Task<(string Token, DateTimeOffset ExpiresAt)> GenerateAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (string.IsNullOrWhiteSpace(_options.SigningKey))
            throw new InvalidOperationException("JWT signing key is not configured.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        if (!string.IsNullOrWhiteSpace(user.UserName))
        {
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName));
        }

        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        }

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var expiresAt = _clock.UtcNow.AddMinutes(_options.AccessTokenLifetimeMinutes);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt.UtcDateTime,
            Issuer = string.IsNullOrWhiteSpace(_options.Issuer) ? null : _options.Issuer,
            Audience = string.IsNullOrWhiteSpace(_options.Audience) ? null : _options.Audience,
            SigningCredentials = credentials
        };

        var token = _tokenHandler.CreateToken(descriptor);
        var serialized = _tokenHandler.WriteToken(token);

        return (serialized, expiresAt);
    }
}
