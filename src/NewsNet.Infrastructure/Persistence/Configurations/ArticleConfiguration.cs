using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsNet.Domain.Entities;
using NewsNet.Domain.ValueObjects;

namespace NewsNet.Infrastructure.Persistence.Configurations;

public sealed class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("articles");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnName("id");

        builder.Property(a => a.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(a => a.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.Property(a => a.PublishedAt)
            .HasColumnName("published_at");

        builder.Property(a => a.AuthorId)
            .HasColumnName("author_id")
            .IsRequired();

        builder.Property(a => a.Views)
            .HasColumnName("views")
            .IsRequired();

        builder.Property(a => a.Title)
            .HasColumnName("title")
            .HasMaxLength(160)
            .IsRequired();

        builder.Property(a => a.Summary)
            .HasColumnName("summary")
            .HasMaxLength(512);

        builder.Property(a => a.Content)
            .HasColumnName("content")
            .IsRequired();

        builder.Property(a => a.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(a => a.Slug)
            .HasColumnName("slug")
            .HasConversion(
                slug => slug.Value,
                value => Slug.FromExisting(value))
            .HasMaxLength(160)
            .IsRequired();

        builder.HasIndex(a => a.Slug)
            .IsUnique();

        builder.HasMany<Comment>()
            .WithOne()
            .HasForeignKey(c => c.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
