using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations.Deposit;

/// <summary>
/// Entity Framework configuration for InterestTier entity
/// </summary>
public class InterestTierConfiguration : IEntityTypeConfiguration<InterestTier>
{
    public void Configure(EntityTypeBuilder<InterestTier> builder)
    {
        builder.ToTable("InterestTiers");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.TierName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.MinimumBalance)
            .HasPrecision(18, 2);

        builder.Property(t => t.MaximumBalance)
            .HasPrecision(18, 2);

        builder.Property(t => t.InterestRate)
            .HasPrecision(5, 4);

        builder.Property(t => t.TierBasis)
            .HasConversion<int>();

        // Relationships
        builder.HasOne(t => t.DepositProduct)
            .WithMany(p => p.InterestTiers)
            .HasForeignKey(t => t.DepositProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(t => t.DepositProductId);
        builder.HasIndex(t => new { t.DepositProductId, t.IsActive });
        builder.HasIndex(t => new { t.DepositProductId, t.MinimumBalance });
        builder.HasIndex(t => t.DisplayOrder);
    }
}