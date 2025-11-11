namespace NewsNet.Application.Common.Models;

public record PagedList<T>(IReadOnlyList<T> Items, int Page, int PageSize, long TotalCount)
{
    public static PagedList<T> Empty(int page, int pageSize) => new(Array.Empty<T>(), page, pageSize, 0);
}
