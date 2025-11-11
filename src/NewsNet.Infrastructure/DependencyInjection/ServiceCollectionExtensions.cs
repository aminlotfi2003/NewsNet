using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsNet.Application.Abstractions.Persistence;
using NewsNet.Application.Abstractions.Persistence.Repositories;
using NewsNet.Domain.Abstractions;
using NewsNet.Infrastructure.Persistence;
using NewsNet.Infrastructure.Persistence.Repositories;
using NewsNet.Infrastructure.Services;

namespace NewsNet.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Postgres connection string is not configured.");

        services.AddDbContext<NewsNetDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<NewsNetDbContext>());
        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IUniqueSlugChecker, ArticleSlugChecker>();
        services.AddSingleton<IClock, SystemClock>();

        return services;
    }
}
