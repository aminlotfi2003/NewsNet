using FluentAssertions;
using NewsNet.Domain.Entities;
using NewsNet.Domain.Enums;
using NewsNet.Domain.Common;

namespace NewsNet.Tests.Domain;

public class UserTests
{
    [Fact]
    public void Create_should_normalize_email_and_set_role()
    {
        var now = DateTimeOffset.UtcNow;
        var u = User.Create("  USER@example.com  ", "HASH", UserRole.User, now);

        u.Email.Should().Be("user@example.com");
        u.Role.Should().Be(UserRole.User);
        u.CreatedAt.Should().Be(now);
    }

    [Fact]
    public void Create_should_throw_when_email_invalid()
    {
        var act = () => User.Create("not-an-email", "HASH", UserRole.User, DateTimeOffset.UtcNow);
        act.Should().Throw<DomainException>().Which.Code.Should().Be("user.email.invalid");
    }

    [Fact]
    public void SetPasswordHash_should_validate()
    {
        var u = User.Create("user@example.com", "HASH", UserRole.User, DateTimeOffset.UtcNow);
        var act = () => u.SetPasswordHash("");
        act.Should().Throw<DomainException>().Which.Code.Should().Be("user.passwordhash.empty");
    }
}
