using Microsoft.EntityFrameworkCore;
using NewsNet.Application.Abstractions.Persistence.Repositories;
using NewsNet.Domain.Entities;

namespace NewsNet.Infrastructure.Persistence.Repositories;

public sealed class CommentRepository : ICommentRepository
{
    private readonly NewsNetDbContext _context;

    public CommentRepository(NewsNetDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        await _context.Comments.AddAsync(comment, cancellationToken);
    }

    public async Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<Comment> Items, long TotalCount)> GetPagedByArticleAsync(
        Guid articleId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Comments
            .AsNoTracking()
            .Where(c => c.ArticleId == articleId)
            .OrderByDescending(c => c.CreatedAt);

        var totalCount = await query.LongCountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Comment>(), 0);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
