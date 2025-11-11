using NewsNet.Domain.Abstractions;

namespace NewsNet.Tests.Fakes;

public sealed class FakeClock : IClock
{
    public DateTimeOffset UtcNow { get; private set; }

    public FakeClock(DateTimeOffset seed) => UtcNow = seed;

    public void Advance(TimeSpan span) => UtcNow = UtcNow.Add(span);
}
