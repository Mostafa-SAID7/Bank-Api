using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for BillPayment entity
/// </summary>
public class BillPaymentConfiguration : IEntityTypeConfiguration<BillPayment>
{
    public void Configure(EntityTypeBuilder<BillPayment> builder)
    {
        // Table name
        builder.ToTable("BillPayments");

        // Indexes for performance
        builder.HasIndex(bp => bp.CustomerId)
            .HasDatabaseName("IX_BillPayments_CustomerId");

        builder.HasIndex(bp => bp.BillerId)
            .HasDatabaseName("IX_BillPayments_BillerId");

        builder.HasIndex(bp => bp.Status)
            .HasDatabaseName("IX_BillPayments_Status");

        builder.HasIndex(bp => bp.ScheduledDate)
            .HasDatabaseName("IX_BillPayments_ScheduledDate");

        builder.HasIndex(bp => new { bp.CustomerId, bp.Status })
            .HasDatabaseName("IX_BillPayments_CustomerId_Status");

        builder.HasIndex(bp => new { bp.Status, bp.ScheduledDate })
            .HasDatabaseName("IX_BillPayments_Status_ScheduledDate");

        builder.HasIndex(bp => bp.RecurringPaymentId)
            .HasDatabaseName("IX_BillPayments_RecurringPaymentId");

        // String property configurations
        builder.Property(bp => bp.Currency)
            .HasMaxLength(3)
            .HasDefaultValue("USD")
            .IsRequired();

        builder.Property(bp => bp.Reference)
            .HasMaxLength(100)
            .HasDefaultValue(string.Empty);

        builder.Property(bp => bp.Description)
            .HasMaxLength(500)
            .HasDefaultValue(string.Empty);

        // Decimal property configurations
        builder.Property(bp => bp.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        // Enum configuration
        builder.Property(bp => bp.Status)
            .HasConversion<int>()
            .HasDefaultValue(BillPaymentStatus.Pending);

        // Relationships
        builder.HasOne(bp => bp.Customer)
            .WithMany()
            .HasForeignKey(bp => bp.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(bp => bp.Biller)
            .WithMany(b => b.BillPayments)
            .HasForeignKey(bp => bp.BillerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(bp => bp.RecurringPayment)
            .WithMany()
            .HasForeignKey(bp => bp.RecurringPaymentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

