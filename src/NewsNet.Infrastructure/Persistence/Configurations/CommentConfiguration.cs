using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsNet.Domain.Entities;

namespace NewsNet.Infrastructure.Persistence.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> b)
    {
        b.ToTable("comments");

        b.HasKey(c => c.Id);

        b.Property(c => c.ArticleId).IsRequired();
        b.Property(c => c.UserId).IsRequired();

        b.Property(c => c.Body)
            .IsRequired()
            .HasMaxLength(2000);

        b.Property(c => c.Status).IsRequired();

        b.Property(c => c.CreatedAt).IsRequired();

        b.HasIndex(c => new { c.ArticleId, c.Status })
            .HasDatabaseName("ix_comments_article_id_status");
    }
}
