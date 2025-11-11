namespace NewsNet.Domain.Common;

public abstract class EntityBase<TId>
{
    public TId Id { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
