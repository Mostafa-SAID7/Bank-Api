using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for FixedDeposit entity
/// </summary>
public class FixedDepositConfiguration : IEntityTypeConfiguration<FixedDeposit>
{
    public void Configure(EntityTypeBuilder<FixedDeposit> builder)
    {
        builder.ToTable("FixedDeposits");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.DepositNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.PrincipalAmount)
            .HasPrecision(18, 2);

        builder.Property(d => d.InterestRate)
            .HasPrecision(5, 4);

        builder.Property(d => d.Status)
            .HasConversion<int>();

        builder.Property(d => d.InterestCalculationMethod)
            .HasConversion<int>();

        builder.Property(d => d.CompoundingFrequency)
            .HasConversion<int>();

        builder.Property(d => d.AccruedInterest)
            .HasPrecision(18, 2);

        builder.Property(d => d.MaturityAction)
            .HasConversion<int>();

        builder.Property(d => d.PenaltyType)
            .HasConversion<int>();

        builder.Property(d => d.PenaltyAmount)
            .HasPrecision(18, 2);

        builder.Property(d => d.PenaltyPercentage)
            .HasPrecision(5, 4);

        builder.Property(d => d.PenaltyApplied)
            .HasPrecision(18, 2);

        builder.Property(d => d.NetAmountPaid)
            .HasPrecision(18, 2);

        builder.Property(d => d.ClosureReason)
            .HasMaxLength(500);

        // Relationships
        builder.HasOne(d => d.Customer)
            .WithMany()
            .HasForeignKey(d => d.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.DepositProduct)
            .WithMany(p => p.FixedDeposits)
            .HasForeignKey(d => d.DepositProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.LinkedAccount)
            .WithMany()
            .HasForeignKey(d => d.LinkedAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.ClosedByUser)
            .WithMany()
            .HasForeignKey(d => d.ClosedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(d => d.RenewedFromDeposit)
            .WithOne(d => d.RenewedToDeposit)
            .HasForeignKey<FixedDeposit>(d => d.RenewedFromDepositId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(d => d.Transactions)
            .WithOne(t => t.FixedDeposit)
            .HasForeignKey(t => t.FixedDepositId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Certificates)
            .WithOne(c => c.FixedDeposit)
            .HasForeignKey(c => c.FixedDepositId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.MaturityNotices)
            .WithOne(n => n.FixedDeposit)
            .HasForeignKey(n => n.FixedDepositId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(d => d.DepositNumber)
            .IsUnique();

        builder.HasIndex(d => d.CustomerId);
        builder.HasIndex(d => d.Status);
        builder.HasIndex(d => d.MaturityDate);
        builder.HasIndex(d => new { d.Status, d.MaturityDate });
        builder.HasIndex(d => new { d.CustomerId, d.Status });
    }
}
