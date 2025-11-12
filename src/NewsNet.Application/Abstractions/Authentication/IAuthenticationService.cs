using NewsNet.Application.Common.Models;

namespace NewsNet.Application.Abstractions.Authentication;

public interface IAuthenticationService
{
    Task<AuthenticationResult?> SignInAsync(
        string userNameOrEmail,
        string password,
        string? ipAddress,
        CancellationToken cancellationToken = default);

    Task<AuthenticationResult?> RefreshTokenAsync(
        string refreshToken,
        string? ipAddress,
        CancellationToken cancellationToken = default);
}
