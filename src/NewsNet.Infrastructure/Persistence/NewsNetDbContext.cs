using Microsoft.EntityFrameworkCore;
using NewsNet.Application.Abstractions.Persistence;
using NewsNet.Domain.Entities;

namespace NewsNet.Infrastructure.Persistence;

public sealed class NewsNetDbContext(DbContextOptions<NewsNetDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<Comment> Comments => Set<Comment>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => base.SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NewsNetDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
