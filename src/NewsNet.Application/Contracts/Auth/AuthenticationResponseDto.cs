namespace NewsNet.Application.Contracts.Auth;

public sealed record AuthenticationResponseDto(
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAt,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAt,
    Guid UserId,
    string UserName,
    string? Email,
    IReadOnlyCollection<string> Roles
);
