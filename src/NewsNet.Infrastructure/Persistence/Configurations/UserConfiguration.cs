using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsNet.Domain.Entities;

namespace NewsNet.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("users");

        b.HasKey(x => x.Id);

        b.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(256);

        b.HasIndex(x => x.Email)
            .IsUnique()
            .HasDatabaseName("ix_users_email");

        b.Property(x => x.PasswordHash)
            .IsRequired();

        b.Property(x => x.Role)
            .IsRequired();

        b.Property(x => x.CreatedAt)
            .IsRequired();
    }
}
