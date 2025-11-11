using Microsoft.EntityFrameworkCore;
using NewsNet.Domain.Entities;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace NewsNet.Infrastructure.Persistence;

public class NewsNetDbContext : DbContext
{
    public NewsNetDbContext(DbContextOptions<NewsNetDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
