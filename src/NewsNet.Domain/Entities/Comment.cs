using NewsNet.Domain.Common;
using NewsNet.Domain.Enums;

namespace NewsNet.Domain.Entities;

public class Comment : EntityBase<Guid>
{
    public Guid ArticleId { get; private set; }
    public Guid UserId { get; private set; }
    public string Body { get; private set; } = default!;
    public CommentStatus Status { get; private set; }

    private Comment(Guid id, Guid articleId, Guid userId, string body, CommentStatus status, DateTimeOffset createdAt)
    {
        Id = id;
        ArticleId = articleId;
        UserId = userId;
        Body = body;
        Status = status;
        CreatedAt = createdAt;
    }

    private Comment()
    {
    }

    public static Comment Create(Guid articleId, Guid userId, string body, DateTimeOffset createdAt)
    {
        if (articleId == Guid.Empty)
            throw new DomainException("comment.article.empty", "ArticleId cannot be empty.");
        if (userId == Guid.Empty)
            throw new DomainException("comment.user.empty", "UserId cannot be empty.");
        if (string.IsNullOrWhiteSpace(body))
            throw new DomainException("comment.body.empty", "Body cannot be empty.");

        var trimmed = body.Trim();
        if (trimmed.Length < 5 || trimmed.Length > 2000)
            throw new DomainException("comment.body.length", "Body length must be 5..2000.");

        return new Comment(Guid.NewGuid(), articleId, userId, trimmed, CommentStatus.Pending, createdAt);
    }

    public void Approve()
    {
        if (Status == CommentStatus.Approved) return;
        Status = CommentStatus.Approved;
    }

    public void Reject()
    {
        if (Status == CommentStatus.Rejected) return;
        Status = CommentStatus.Rejected;
    }
}
