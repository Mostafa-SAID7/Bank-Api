using Bank.Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations.Payment;

/// <summary>
/// Entity Framework configuration for RecurringPayment entity
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
