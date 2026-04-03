using Bank.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations.Auth;

/// <summary>
/// Entity Framework configuration for IpWhitelist entity
/// </summary>
public class IpWhitelistConfiguration : IEntityTypeConfiguration<IpWhitelist>
{
    public void Configure(EntityTypeBuilder<IpWhitelist> builder)
    {
        builder.HasOne(w => w.CreatedByUser)
            .WithMany()
            .HasForeignKey(w => w.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(w => w.ApprovedByUser)
            .WithMany()
            .HasForeignKey(w => w.ApprovedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(w => new { w.IpAddress, w.Type })
            .HasDatabaseName("IX_IpWhitelists_IpAddress_Type");

        builder.HasIndex(w => new { w.Type, w.IsActive })
            .HasDatabaseName("IX_IpWhitelists_Type_IsActive");

        builder.HasIndex(w => w.ExpiresAt)
            .HasDatabaseName("IX_IpWhitelists_ExpiresAt");

        builder.Property(w => w.IpAddress)
            .HasMaxLength(45)
            .IsRequired();

        builder.Property(w => w.IpRange)
            .HasMaxLength(50);

        builder.Property(w => w.Description)
            .HasMaxLength(500)
            .IsRequired();
    }
}
