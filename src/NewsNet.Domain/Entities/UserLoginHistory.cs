using NewsNet.Domain.Common;

namespace NewsNet.Domain.Entities;

public class UserLoginHistory : EntityBase<Guid>
{
    public Guid UserId { get; set; }
    public DateTimeOffset OccurredAt { get; set; } = DateTimeOffset.UtcNow;
    public string? IpAddress { get; set; }
    public string? Host { get; set; }
    public bool Success { get; set; }
    public int FailureCountBeforeSuccess { get; set; }
}
