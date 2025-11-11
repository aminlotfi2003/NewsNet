namespace NewsNet.Domain.Abstractions;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
