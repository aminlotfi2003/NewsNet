using Microsoft.Extensions.DependencyInjection;

namespace NewsNet.Infrastructure.Persistence;

public static class DatabaseInitializationExtensions
{
    public static async Task InitialiseDatabaseAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(services);

        await using var scope = services.CreateAsyncScope();
        var initializer = scope.ServiceProvider.GetRequiredService<NewsNetDbInitializer>();
        await initializer.InitialiseAsync(cancellationToken);
    }
}
