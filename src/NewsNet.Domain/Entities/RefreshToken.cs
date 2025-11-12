using NewsNet.Domain.Common;

namespace NewsNet.Domain.Entities;

public class RefreshToken : EntityBase<Guid>
{
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = default!;
    public string TokenHash { get; set; } = default!;
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset? RevokedAt { get; private set; }
    public string? ReplacedByTokenHash { get; private set; }
    public string? CreatedByIp { get; set; }
    public string? RevokedByIp { get; private set; }
    public string? ReasonRevoked { get; private set; }

    public bool IsActive => RevokedAt is null && DateTimeOffset.UtcNow < ExpiresAt;

    public void Revoke(DateTimeOffset revokedAt, string? revokedByIp, string? reason, string? replacedByTokenHash)
    {
        RevokedAt = revokedAt;
        RevokedByIp = revokedByIp;
        ReasonRevoked = reason;
        ReplacedByTokenHash = replacedByTokenHash;
    }
}
