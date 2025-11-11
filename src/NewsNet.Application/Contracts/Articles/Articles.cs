using NewsNet.Domain.Enums;

namespace NewsNet.Application.Contracts.Articles;

public record ArticleDto(
    Guid Id,
    string Slug,
    string Title,
    string? Summary,
    string Content,
    ArticleStatus Status,
    Guid AuthorId,
    DateTimeOffset? PublishedAt,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    long Views
);
