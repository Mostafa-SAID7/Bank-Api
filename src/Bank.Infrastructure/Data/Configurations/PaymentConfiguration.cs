using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for payment-related entities
/// </summary>
public class RecurringPaymentConfiguration : IEntityTypeConfiguration<RecurringPayment>
{
    public void Configure(EntityTypeBuilder<RecurringPayment> builder)
    {
        builder.HasOne(rp => rp.FromAccount)
            .WithMany()
            .HasForeignKey(rp => rp.FromAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(rp => rp.ToAccount)
            .WithMany()
            .HasForeignKey(rp => rp.ToAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(rp => rp.CreatedByUser)
            .WithMany()
            .HasForeignKey(rp => rp.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(rp => rp.Amount)
            .HasPrecision(18, 2);

        builder.HasIndex(rp => new { rp.Status, rp.NextExecutionDate })
            .HasDatabaseName("IX_RecurringPayments_Status_NextExecutionDate");

        builder.HasIndex(rp => rp.CreatedByUserId)
            .HasDatabaseName("IX_RecurringPayments_CreatedByUserId");
    }
}

public class RecurringPaymentExecutionConfiguration : IEntityTypeConfiguration<RecurringPaymentExecution>
{
    public void Configure(EntityTypeBuilder<RecurringPaymentExecution> builder)
    {
        builder.HasOne(rpe => rpe.RecurringPayment)
            .WithMany(rp => rp.Executions)
            .HasForeignKey(rpe => rpe.RecurringPaymentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(rpe => rpe.Transaction)
            .WithMany()
            .HasForeignKey(rpe => rpe.TransactionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(rpe => rpe.Amount)
            .HasPrecision(18, 2);

        builder.HasIndex(rpe => new { rpe.RecurringPaymentId, rpe.ScheduledDate })
            .HasDatabaseName("IX_RecurringPaymentExecutions_RecurringPaymentId_ScheduledDate");
    }
}

public class PaymentTemplateConfiguration : IEntityTypeConfiguration<PaymentTemplate>
{
    public void Configure(EntityTypeBuilder<PaymentTemplate> builder)
    {
        builder.HasOne(pt => pt.FromAccount)
            .WithMany()
            .HasForeignKey(pt => pt.FromAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pt => pt.ToAccount)
            .WithMany()
            .HasForeignKey(pt => pt.ToAccountId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(pt => pt.CreatedByUser)
            .WithMany()
            .HasForeignKey(pt => pt.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(pt => pt.Amount)
            .HasPrecision(18, 2);

        // Indexes for performance
        builder.HasIndex(pt => new { pt.CreatedByUserId, pt.IsActive })
            .HasDatabaseName("IX_PaymentTemplates_CreatedByUserId_IsActive");

        builder.HasIndex(pt => new { pt.CreatedByUserId, pt.Category })
            .HasDatabaseName("IX_PaymentTemplates_CreatedByUserId_Category");

        builder.HasIndex(pt => new { pt.CreatedByUserId, pt.UsageCount })
            .HasDatabaseName("IX_PaymentTemplates_CreatedByUserId_UsageCount");

        builder.HasIndex(pt => new { pt.CreatedByUserId, pt.LastUsedDate })
            .HasDatabaseName("IX_PaymentTemplates_CreatedByUserId_LastUsedDate");

        // String property configurations
        builder.Property(pt => pt.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(pt => pt.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(pt => pt.BeneficiaryName)
            .HasMaxLength(200);

        builder.Property(pt => pt.BeneficiaryAccountNumber)
            .HasMaxLength(50);

        builder.Property(pt => pt.BeneficiaryBankCode)
            .HasMaxLength(20);

        builder.Property(pt => pt.Reference)
            .HasMaxLength(100);

        builder.Property(pt => pt.Notes)
            .HasMaxLength(1000);

        builder.Property(pt => pt.Tags)
            .HasMaxLength(1000);
    }
}