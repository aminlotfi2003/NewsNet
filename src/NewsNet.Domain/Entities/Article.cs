using NewsNet.Domain.Common;
using NewsNet.Domain.Enums;
using NewsNet.Domain.ValueObjects;

namespace NewsNet.Domain.Entities;

public class Article
{
    public Guid Id { get; private set; }
    public Slug Slug { get; private set; }
    public string Title { get; private set; }
    public string? Summary { get; private set; }
    public string Content { get; private set; }
    public IReadOnlyList<string> Tags => _tags.AsReadOnly();
    private readonly List<string> _tags = new();
    public ArticleStatus Status { get; private set; }
    public Guid AuthorId { get; private set; }
    public DateTimeOffset? PublishedAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public long Views { get; private set; }

    private Article(
        Guid id,
        Slug slug,
        string title,
        string? summary,
        string content,
        IEnumerable<string> tags,
        ArticleStatus status,
        Guid authorId,
        DateTimeOffset? publishedAt,
        DateTimeOffset createdAt,
        DateTimeOffset updatedAt,
        long views)
    {
        Id = id;
        Slug = slug;
        Title = title;
        Summary = summary;
        Content = content;
        _tags = tags.ToList();
        Status = status;
        AuthorId = authorId;
        PublishedAt = publishedAt;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Views = views;
    }

    public static Article Create(
        Slug slug,
        string title,
        string? summary,
        string content,
        IEnumerable<string> tags,
        Guid authorId,
        DateTimeOffset createdAt)
    {
        ValidateTitle(title);
        ValidateContent(content);
        var tagList = ValidateAndNormalizeTags(tags);

        return new Article(
            Guid.NewGuid(),
            slug,
            title.Trim(),
            string.IsNullOrWhiteSpace(summary) ? null : summary.Trim(),
            content,
            tagList,
            ArticleStatus.Draft,
            authorId,
            null,
            createdAt,
            createdAt,
            0
        );
    }

    public void Update(
        string title,
        string? summary,
        string content,
        IEnumerable<string> tags,
        DateTimeOffset updatedAt)
    {
        ValidateTitle(title);
        ValidateContent(content);
        var tagList = ValidateAndNormalizeTags(tags);

        Title = title.Trim();
        Summary = string.IsNullOrWhiteSpace(summary) ? null : summary.Trim();
        Content = content;
        _tags.Clear();
        _tags.AddRange(tagList);
        UpdatedAt = updatedAt;
    }

    public void ChangeSlug(Slug slug, DateTimeOffset updatedAt)
    {
        Slug = slug;
        UpdatedAt = updatedAt;
    }

    public void Publish(DateTimeOffset nowUtc)
    {
        if (Status == ArticleStatus.Published)
            throw new DomainException("article.publish.already", "Article already published.");

        if (Content is null || Content.Trim().Length < 50)
            throw new DomainException("article.publish.content_short", "Content must be at least 50 characters for publishing.");

        Status = ArticleStatus.Published;
        PublishedAt = nowUtc;
    }

    public void IncrementViews(long amount = 1)
    {
        if (amount <= 0) return;
        checked
        {
            Views += amount;
        }
    }

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("article.title.empty", "Title cannot be empty.");

        var trimmed = title.Trim();
        if (trimmed.Length < 5 || trimmed.Length > 160)
            throw new DomainException("article.title.length", "Title length must be between 5 and 160.");
    }

    private static void ValidateContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new DomainException("article.content.empty", "Content cannot be empty.");

        if (content.Trim().Length < 50)
            throw new DomainException("article.content.length", "Content length must be ≥ 50.");
    }

    private static List<string> ValidateAndNormalizeTags(IEnumerable<string> tags)
    {
        var list = (tags ?? Enumerable.Empty<string>())
            .Select(t => t?.Trim())
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(t => t!)
            .ToList();

        if (list.Count > 10)
            throw new DomainException("article.tags.count", "Tags count cannot exceed 10.");

        foreach (var tag in list)
        {
            if (tag.Length > 24)
                throw new DomainException("article.tags.length", "Each tag length must be ≤ 24.");
        }

        return list;
    }
}
