using NewsNet.Domain.Abstractions;

namespace NewsNet.Infrastructure.Services;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
