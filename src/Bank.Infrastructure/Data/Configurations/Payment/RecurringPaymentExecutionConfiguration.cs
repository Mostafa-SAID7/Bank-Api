using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for RecurringPaymentExecution entity
/// </summary>
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

