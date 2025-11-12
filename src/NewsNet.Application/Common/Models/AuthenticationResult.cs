namespace NewsNet.Application.Common.Models;

public sealed record AuthenticationResult(
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAt,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAt,
    Guid UserId,
    string UserName,
    string? Email,
    IReadOnlyCollection<string> Roles
);
