using NewsNet.Domain.Entities;

namespace NewsNet.Application.Abstractions.Persistence.Repositories;

public interface ICommentRepository
{
    Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Comment> Items, long TotalCount)> GetPagedByArticleAsync(
        Guid articleId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task AddAsync(Comment comment, CancellationToken cancellationToken = default);
}
