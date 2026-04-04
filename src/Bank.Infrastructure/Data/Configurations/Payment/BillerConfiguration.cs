using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Biller entity
/// </summary>
public class BillerConfiguration : IEntityTypeConfiguration<Biller>
{
    public void Configure(EntityTypeBuilder<Biller> builder)
    {
        // Table name
        builder.ToTable("Billers");

        // Indexes for performance
        builder.HasIndex(b => b.IsActive)
            .HasDatabaseName("IX_Billers_IsActive");

        builder.HasIndex(b => b.Category)
            .HasDatabaseName("IX_Billers_Category");

        builder.HasIndex(b => new { b.AccountNumber, b.RoutingNumber })
            .HasDatabaseName("IX_Billers_AccountNumber_RoutingNumber")
            .IsUnique();

        builder.HasIndex(b => b.Name)
            .HasDatabaseName("IX_Billers_Name");

        // String property configurations
        builder.Property(b => b.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(b => b.AccountNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(b => b.RoutingNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(b => b.Address)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(b => b.SupportedPaymentMethods)
            .HasMaxLength(1000)
            .HasDefaultValue("[]");

        // Decimal property configurations
        builder.Property(b => b.MinAmount)
            .HasPrecision(18, 2)
            .HasDefaultValue(0.01m);

        builder.Property(b => b.MaxAmount)
            .HasPrecision(18, 2)
            .HasDefaultValue(10000.00m);

        // Enum configuration
        builder.Property(b => b.Category)
            .HasConversion<int>();

        // Default values
        builder.Property(b => b.IsActive)
            .HasDefaultValue(true);

        builder.Property(b => b.ProcessingDays)
            .HasDefaultValue(1);

        // Relationships
        builder.HasMany(b => b.BillPayments)
            .WithOne(bp => bp.Biller)
            .HasForeignKey(bp => bp.BillerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

