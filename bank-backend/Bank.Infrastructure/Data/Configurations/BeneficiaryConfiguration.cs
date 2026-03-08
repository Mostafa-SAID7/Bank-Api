using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Beneficiary entity
/// </summary>
public class BeneficiaryConfiguration : IEntityTypeConfiguration<Beneficiary>
{
    public void Configure(EntityTypeBuilder<Beneficiary> builder)
    {
        // Relationships
        builder.HasOne(b => b.Customer)
            .WithMany()
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.VerifiedByUser)
            .WithMany()
            .HasForeignKey(b => b.VerifiedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes for performance
        builder.HasIndex(b => new { b.CustomerId, b.IsActive })
            .HasDatabaseName("IX_Beneficiaries_CustomerId_IsActive");

        builder.HasIndex(b => new { b.CustomerId, b.AccountNumber, b.BankCode })
            .HasDatabaseName("IX_Beneficiaries_CustomerId_AccountNumber_BankCode");

        builder.HasIndex(b => new { b.CustomerId, b.Category })
            .HasDatabaseName("IX_Beneficiaries_CustomerId_Category");

        builder.HasIndex(b => new { b.CustomerId, b.Type })
            .HasDatabaseName("IX_Beneficiaries_CustomerId_Type");

        builder.HasIndex(b => b.Status)
            .HasDatabaseName("IX_Beneficiaries_Status");

        builder.HasIndex(b => b.IsVerified)
            .HasDatabaseName("IX_Beneficiaries_IsVerified");

        builder.HasIndex(b => b.LastTransferDate)
            .HasDatabaseName("IX_Beneficiaries_LastTransferDate");

        // String property configurations
        builder.Property(b => b.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(b => b.AccountNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(b => b.AccountName)
            .HasMaxLength(200);

        builder.Property(b => b.BankName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(b => b.BankCode)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(b => b.SwiftCode)
            .HasMaxLength(11);

        builder.Property(b => b.IbanNumber)
            .HasMaxLength(34);

        builder.Property(b => b.RoutingNumber)
            .HasMaxLength(20);

        builder.Property(b => b.Notes)
            .HasMaxLength(1000);

        builder.Property(b => b.Reference)
            .HasMaxLength(100);

        builder.Property(b => b.ArchiveReason)
            .HasMaxLength(500);

        // Decimal property configurations
        builder.Property(b => b.DailyTransferLimit)
            .HasPrecision(18, 2);

        builder.Property(b => b.MonthlyTransferLimit)
            .HasPrecision(18, 2);

        builder.Property(b => b.SingleTransferLimit)
            .HasPrecision(18, 2);

        builder.Property(b => b.TotalTransferAmount)
            .HasPrecision(18, 2);
    }
}