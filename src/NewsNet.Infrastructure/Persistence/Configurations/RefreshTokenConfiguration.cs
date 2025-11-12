using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsNet.Domain.Entities;

namespace NewsNet.Infrastructure.Persistence.Configurations;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.HasKey(token => token.Id);

        builder.Property(token => token.Id)
            .HasColumnName("id");

        builder.Property(token => token.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(token => token.TokenHash)
            .HasColumnName("token_hash")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(token => token.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(token => token.ExpiresAt)
            .HasColumnName("expires_at")
            .IsRequired();

        builder.Property(token => token.RevokedAt)
            .HasColumnName("revoked_at");

        builder.Property(token => token.ReplacedByTokenHash)
            .HasColumnName("replaced_by_token_hash")
            .HasMaxLength(512);

        builder.Property(token => token.CreatedByIp)
            .HasColumnName("created_by_ip")
            .HasMaxLength(64);

        builder.Property(token => token.RevokedByIp)
            .HasColumnName("revoked_by_ip")
            .HasMaxLength(64);

        builder.Property(token => token.ReasonRevoked)
            .HasColumnName("reason_revoked")
            .HasMaxLength(256);

        builder.HasOne(token => token.User)
            .WithMany(user => user.RefreshTokens)
            .HasForeignKey(token => token.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(token => token.TokenHash)
            .IsUnique();
    }
}
