using NewsNet.Domain.Enums;

namespace NewsNet.Application.Contracts.Comments;

public record CommentDto(
    Guid Id,
    Guid ArticleId,
    Guid UserId,
    string Body,
    CommentStatus Status,
    DateTimeOffset CreatedAt
);
