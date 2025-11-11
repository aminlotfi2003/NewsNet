using NewsNet.Domain.Entities;

namespace NewsNet.Application.Abstractions.Persistence.Repositories;

public interface IArticleRepository
{
    Task<Article?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Article?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Article> Items, long TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task AddAsync(Article article, CancellationToken cancellationToken = default);
    void Remove(Article article);
}
