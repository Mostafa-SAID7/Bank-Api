using Bank.Application.DTOs;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for bill payment entities
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
public class BillPresentmentConfiguration : IEntityTypeConfiguration<BillPresentment>
{
    public void Configure(EntityTypeBuilder<BillPresentment> builder)
    {
        // Table name
        builder.ToTable("BillPresentments");

        // Indexes for performance
        builder.HasIndex(bp => bp.CustomerId)
            .HasDatabaseName("IX_BillPresentments_CustomerId");

        builder.HasIndex(bp => bp.BillerId)
            .HasDatabaseName("IX_BillPresentments_BillerId");

        builder.HasIndex(bp => bp.Status)
            .HasDatabaseName("IX_BillPresentments_Status");

        builder.HasIndex(bp => bp.DueDate)
            .HasDatabaseName("IX_BillPresentments_DueDate");

        builder.HasIndex(bp => bp.ExternalBillId)
            .IsUnique()
            .HasDatabaseName("IX_BillPresentments_ExternalBillId");

        builder.HasIndex(bp => new { bp.CustomerId, bp.Status })
            .HasDatabaseName("IX_BillPresentments_CustomerId_Status");

        // String property configurations
        builder.Property(bp => bp.AccountNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(bp => bp.Currency)
            .IsRequired()
            .HasMaxLength(3)
            .HasDefaultValue("USD");

        builder.Property(bp => bp.BillNumber)
            .HasMaxLength(100);

        builder.Property(bp => bp.ExternalBillId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(bp => bp.LineItemsJson)
            .HasColumnType("nvarchar(max)");

        // Decimal configurations
        builder.Property(bp => bp.AmountDue)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(bp => bp.MinimumPayment)
            .IsRequired()
            .HasPrecision(18, 2);

        // Enum configurations
        builder.Property(bp => bp.Status)
            .HasConversion<int>()
            .HasDefaultValue(Bank.Application.DTOs.BillPresentmentStatus.Pending);

        // Relationships
        builder.HasOne(bp => bp.Customer)
            .WithMany()
            .HasForeignKey(bp => bp.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(bp => bp.Biller)
            .WithMany()
            .HasForeignKey(bp => bp.BillerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(bp => bp.Payment)
            .WithMany()
            .HasForeignKey(bp => bp.PaymentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class PaymentReceiptConfiguration : IEntityTypeConfiguration<PaymentReceipt>
{
    public void Configure(EntityTypeBuilder<PaymentReceipt> builder)
    {
        // Table name
        builder.ToTable("PaymentReceipts");

        // Indexes for performance
        builder.HasIndex(pr => pr.PaymentId)
            .IsUnique()
            .HasDatabaseName("IX_PaymentReceipts_PaymentId");

        builder.HasIndex(pr => pr.ReceiptNumber)
            .IsUnique()
            .HasDatabaseName("IX_PaymentReceipts_ReceiptNumber");

        builder.HasIndex(pr => pr.CustomerId)
            .HasDatabaseName("IX_PaymentReceipts_CustomerId");

        builder.HasIndex(pr => pr.ConfirmationNumber)
            .HasDatabaseName("IX_PaymentReceipts_ConfirmationNumber");

        builder.HasIndex(pr => pr.Status)
            .HasDatabaseName("IX_PaymentReceipts_Status");

        // String property configurations
        builder.Property(pr => pr.ReceiptNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(pr => pr.CustomerName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(pr => pr.BillerName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(pr => pr.Currency)
            .IsRequired()
            .HasMaxLength(3)
            .HasDefaultValue("USD");

        builder.Property(pr => pr.ConfirmationNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pr => pr.Reference)
            .HasMaxLength(100);

        builder.Property(pr => pr.ReceiptDataJson)
            .HasColumnType("nvarchar(max)");

        // Decimal configurations
        builder.Property(pr => pr.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(pr => pr.ProcessingFee)
            .HasPrecision(18, 2);

        // Enum configurations
        builder.Property(pr => pr.PaymentMethod)
            .HasConversion<int>();

        builder.Property(pr => pr.Status)
            .HasConversion<int>();

        // Relationships
        builder.HasOne(pr => pr.Payment)
            .WithMany()
            .HasForeignKey(pr => pr.PaymentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pr => pr.Customer)
            .WithMany()
            .HasForeignKey(pr => pr.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class PaymentRetryConfiguration : IEntityTypeConfiguration<PaymentRetry>
{
    public void Configure(EntityTypeBuilder<PaymentRetry> builder)
    {
        // Table name
        builder.ToTable("PaymentRetries");

        // Indexes for performance
        builder.HasIndex(pr => pr.PaymentId)
            .HasDatabaseName("IX_PaymentRetries_PaymentId");

        builder.HasIndex(pr => pr.NextRetryDate)
            .HasDatabaseName("IX_PaymentRetries_NextRetryDate");

        builder.HasIndex(pr => pr.Status)
            .HasDatabaseName("IX_PaymentRetries_Status");

        builder.HasIndex(pr => pr.IsMaxRetriesReached)
            .HasDatabaseName("IX_PaymentRetries_IsMaxRetriesReached");

        builder.HasIndex(pr => new { pr.PaymentId, pr.AttemptNumber })
            .IsUnique()
            .HasDatabaseName("IX_PaymentRetries_PaymentId_AttemptNumber");

        // String property configurations
        builder.Property(pr => pr.FailureReason)
            .HasMaxLength(1000);

        builder.Property(pr => pr.RetryMetadataJson)
            .HasColumnType("nvarchar(max)");

        // Enum configurations
        builder.Property(pr => pr.Status)
            .HasConversion<int>();

        // Default values
        builder.Property(pr => pr.IsMaxRetriesReached)
            .HasDefaultValue(false);

        // Relationships
        builder.HasOne(pr => pr.Payment)
            .WithMany()
            .HasForeignKey(pr => pr.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class BillerHealthCheckConfiguration : IEntityTypeConfiguration<BillerHealthCheck>
{
    public void Configure(EntityTypeBuilder<BillerHealthCheck> builder)
    {
        // Table name
        builder.ToTable("BillerHealthChecks");

        // Indexes for performance
        builder.HasIndex(bhc => bhc.BillerId)
            .HasDatabaseName("IX_BillerHealthChecks_BillerId");

        builder.HasIndex(bhc => bhc.CheckDate)
            .HasDatabaseName("IX_BillerHealthChecks_CheckDate");

        builder.HasIndex(bhc => bhc.IsHealthy)
            .HasDatabaseName("IX_BillerHealthChecks_IsHealthy");

        builder.HasIndex(bhc => new { bhc.BillerId, bhc.CheckDate })
            .HasDatabaseName("IX_BillerHealthChecks_BillerId_CheckDate");

        // String property configurations
        builder.Property(bhc => bhc.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(bhc => bhc.ErrorMessage)
            .HasMaxLength(1000);

        builder.Property(bhc => bhc.HealthMetricsJson)
            .HasColumnType("nvarchar(max)");

        // Default values
        builder.Property(bhc => bhc.IsHealthy)
            .HasDefaultValue(true);

        builder.Property(bhc => bhc.ConsecutiveFailures)
            .HasDefaultValue(0);

        // Relationships
        builder.HasOne(bhc => bhc.Biller)
            .WithMany()
            .HasForeignKey(bhc => bhc.BillerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}