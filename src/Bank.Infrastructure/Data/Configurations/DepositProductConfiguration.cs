using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for DepositProduct entity
/// </summary>
public class DepositProductConfiguration : IEntityTypeConfiguration<DepositProduct>
{
    public void Configure(EntityTypeBuilder<DepositProduct> builder)
    {
        builder.ToTable("DepositProducts");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.ProductType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(p => p.MinimumBalance)
            .HasPrecision(18, 2);

        builder.Property(p => p.MaximumBalance)
            .HasPrecision(18, 2);

        builder.Property(p => p.MinimumOpeningBalance)
            .HasPrecision(18, 2);

        builder.Property(p => p.BaseInterestRate)
            .HasPrecision(5, 4);

        builder.Property(p => p.InterestCalculationMethod)
            .HasConversion<int>();

        builder.Property(p => p.CompoundingFrequency)
            .HasConversion<int>();

        builder.Property(p => p.PenaltyType)
            .HasConversion<int>();

        builder.Property(p => p.PenaltyAmount)
            .HasPrecision(18, 2);

        builder.Property(p => p.PenaltyPercentage)
            .HasPrecision(5, 4);

        builder.Property(p => p.DefaultMaturityAction)
            .HasConversion<int>();

        builder.Property(p => p.PromotionalRate)
            .HasPrecision(5, 4);

        // Relationships
        builder.HasMany(p => p.InterestTiers)
            .WithOne(t => t.DepositProduct)
            .HasForeignKey(t => t.DepositProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.FixedDeposits)
            .WithOne(d => d.DepositProduct)
            .HasForeignKey(d => d.DepositProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(p => p.ProductType);
        builder.HasIndex(p => p.IsActive);
        builder.HasIndex(p => new { p.ProductType, p.IsActive });
    }
}