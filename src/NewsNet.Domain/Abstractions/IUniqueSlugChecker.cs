namespace NewsNet.Domain.Abstractions;

public interface IUniqueSlugChecker
{
    Task<bool> IsUniqueAsync(string slug, CancellationToken ct = default);
}
