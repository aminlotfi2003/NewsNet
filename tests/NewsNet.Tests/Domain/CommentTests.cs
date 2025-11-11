using FluentAssertions;
using NewsNet.Domain.Common;
using NewsNet.Domain.Entities;
using NewsNet.Domain.Enums;

namespace NewsNet.Tests.Domain;

public class CommentTests
{
    [Fact]
    public void Create_should_be_pending_and_validate_length()
    {
        var articleId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var c = Comment.Create(articleId, userId, new string('x', 50), DateTimeOffset.UtcNow);
        c.Status.Should().Be(CommentStatus.Pending);

        var bad = () => Comment.Create(articleId, userId, "bad", DateTimeOffset.UtcNow);
        bad.Should().Throw<DomainException>().Which.Code.Should().Be("comment.body.length");
    }

    [Fact]
    public void Approve_and_Reject_should_change_status()
    {
        var c = Comment.Create(Guid.NewGuid(), Guid.NewGuid(), new string('x', 50), DateTimeOffset.UtcNow);
        c.Approve();
        c.Status.Should().Be(CommentStatus.Approved);

        c.Reject();
        c.Status.Should().Be(CommentStatus.Rejected);
    }
}
