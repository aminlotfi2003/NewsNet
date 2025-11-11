using NewsNet.Domain.Common;

namespace NewsNet.Domain.Entities;

public class PasswordHistory : EntityBase<Guid>
{
    public Guid UserId { get; set; }
    public string PasswordHash { get; set; } = default!;
    public DateTimeOffset ChangedAt { get; set; } = DateTimeOffset.UtcNow;
}
