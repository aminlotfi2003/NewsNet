using Microsoft.EntityFrameworkCore;
using NewsNet.Application.Abstractions.Persistence.Repositories;
using NewsNet.Domain.Entities;
using NewsNet.Domain.ValueObjects;

namespace NewsNet.Infrastructure.Persistence.Repositories;

public sealed class ArticleRepository : IArticleRepository
{
    private readonly NewsNetDbContext _context;

    public ArticleRepository(NewsNetDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Article article, CancellationToken cancellationToken = default)
    {
        await _context.Articles.AddAsync(article, cancellationToken);
    }

    public async Task<Article?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Articles
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Article?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Articles
            .FirstOrDefaultAsync(a => a.Slug == Slug.FromExisting(slug), cancellationToken);
    }

    public async Task<(IReadOnlyList<Article> Items, long TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Articles
            .AsNoTracking()
            .OrderByDescending(a => a.CreatedAt);

        var totalCount = await query.LongCountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Article>(), 0);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public void Remove(Article article)
    {
        _context.Articles.Remove(article);
    }
}
