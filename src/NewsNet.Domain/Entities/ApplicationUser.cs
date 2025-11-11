using Microsoft.AspNetCore.Identity;

namespace NewsNet.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public Guid? TenantId { get; set; }
    public DateTimeOffset? PasswordLastChangedAt { get; set; }
    public bool MustChangePasswordOnFirstLogin { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ModifiedAt { get; set; }
    public bool IsActive { get; set; } = true;
}
