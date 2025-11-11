using Microsoft.EntityFrameworkCore;
using NewsNet.Domain.Abstractions;
using NewsNet.Domain.ValueObjects;
using NewsNet.Infrastructure.Persistence;

namespace NewsNet.Infrastructure.Services;

public sealed class ArticleSlugChecker : IUniqueSlugChecker
{
    private readonly NewsNetDbContext _context;

    public ArticleSlugChecker(NewsNetDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsUniqueAsync(string slug, CancellationToken ct = default)
    {
        return !await _context.Articles
            .AsNoTracking()
            .AnyAsync(a => a.Slug == Slug.FromExisting(slug), ct);
    }
}
