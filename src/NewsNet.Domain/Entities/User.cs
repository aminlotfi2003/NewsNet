using System.Net.Mail;
using NewsNet.Domain.Common;
using NewsNet.Domain.Enums;

namespace NewsNet.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private User(Guid id, string email, string passwordHash, UserRole role, DateTimeOffset createdAt)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = createdAt;
    }

    public static User Create(string email, string passwordHash, UserRole role, DateTimeOffset createdAt)
    {
        email = NormalizeEmail(email);
        if (!IsValidEmail(email))
            throw new DomainException("user.email.invalid", "Email is invalid.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("user.passwordhash.empty", "PasswordHash cannot be empty.");

        return new User(Guid.NewGuid(), email, passwordHash, role, createdAt);
    }

    public void SetPasswordHash(string newHash)
    {
        if (string.IsNullOrWhiteSpace(newHash))
            throw new DomainException("user.passwordhash.empty", "PasswordHash cannot be empty.");
        PasswordHash = newHash;
    }

    public void PromoteToAdmin() => Role = UserRole.Admin;

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();

    private static bool IsValidEmail(string email)
    {
        try
        {
            var _ = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
