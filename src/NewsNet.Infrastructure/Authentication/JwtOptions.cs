namespace NewsNet.Infrastructure.Authentication;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "https://newsnet";
    public string Audience { get; set; } = "https://newsnet.clients";
    public string SigningKey { get; set; } = "";
    public int AccessTokenLifetimeMinutes { get; set; } = 15;
    public int RefreshTokenLifetimeDays { get; set; } = 7;
}
