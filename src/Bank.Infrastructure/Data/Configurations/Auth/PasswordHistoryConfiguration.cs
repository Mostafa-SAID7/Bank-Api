using Bank.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations.Auth;

/// <summary>
/// Entity Framework configuration for PasswordHistory entity
/// </summary>
public class PasswordHistoryConfiguration : IEntityTypeConfiguration<PasswordHistory>
{
    public void Configure(EntityTypeBuilder<PasswordHistory> builder)
    {
        builder.HasOne(h => h.User)
            .WithMany()
            .HasForeignKey(h => h.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(h => new { h.UserId, h.PasswordSetAt })
            .HasDatabaseName("IX_PasswordHistories_UserId_PasswordSetAt");

        builder.HasIndex(h => new { h.UserId, h.IsCurrentPassword })
            .HasDatabaseName("IX_PasswordHistories_UserId_IsCurrentPassword");

        builder.Property(h => h.PasswordHash)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(h => h.PasswordSalt)
            .HasMaxLength(128);
    }
}
