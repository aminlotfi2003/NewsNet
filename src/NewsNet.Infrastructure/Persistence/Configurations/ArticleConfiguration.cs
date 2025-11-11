using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsNet.Domain.Entities;
using NewsNet.Domain.ValueObjects;

namespace NewsNet.Infrastructure.Persistence.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> b)
    {
        b.ToTable("articles");

        b.HasKey(a => a.Id);

        // Slug as ValueObject (string)
        b.Property(a => a.Slug)
            .HasConversion(
                v => v.Value,
                v => Slug.Create(v))
            .IsRequired()
            .HasMaxLength(160);

        b.HasIndex(a => a.Slug)
            .IsUnique()
            .HasDatabaseName("ix_articles_slug");

        b.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(160);

        b.Property(a => a.Summary)
            .HasMaxLength(500);

        b.Property(a => a.Content)
            .IsRequired()
            .HasColumnType("text");

        // Tags -> jsonb
        b.Property<List<string>>("_tags")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("tags")
            .HasColumnType("jsonb")
            .IsRequired();

        b.Property(a => a.Status).IsRequired();

        b.Property(a => a.AuthorId).IsRequired();

        b.Property(a => a.PublishedAt);

        b.Property(a => a.CreatedAt).IsRequired();
        b.Property(a => a.UpdatedAt).IsRequired();

        b.Property(a => a.Views)
            .IsRequired()
            .HasDefaultValue(0L);

        // Composite index for status + publishedAt DESC
        b.HasIndex(a => new { a.Status, a.PublishedAt })
            .IsDescending(false, true)
            .HasDatabaseName("ix_articles_status_published_at_desc");
    }
}
