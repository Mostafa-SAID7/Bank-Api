using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for DepositTransaction entity
/// </summary>
public class DepositTransactionConfiguration : IEntityTypeConfiguration<DepositTransaction>
{
    public void Configure(EntityTypeBuilder<DepositTransaction> builder)
    {
        builder.ToTable("DepositTransactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.TransactionReference)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.TransactionType)
            .HasConversion<int>();

        builder.Property(t => t.Amount)
            .HasPrecision(18, 2);

        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(t => t.Status)
            .HasConversion<int>();

        builder.Property(t => t.InterestRate)
            .HasPrecision(5, 4);

        builder.Property(t => t.PenaltyType)
            .HasConversion<int>();

        builder.Property(t => t.PenaltyReason)
            .HasMaxLength(500);

        builder.Property(t => t.ProcessingNotes)
            .HasMaxLength(1000);

        // Relationships
        builder.HasOne(t => t.FixedDeposit)
            .WithMany(d => d.Transactions)
            .HasForeignKey(t => t.FixedDepositId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.ProcessedByUser)
            .WithMany()
            .HasForeignKey(t => t.ProcessedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(t => t.RelatedTransaction)
            .WithMany()
            .HasForeignKey(t => t.RelatedTransactionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(t => t.AccountTransaction)
            .WithMany()
            .HasForeignKey(t => t.AccountTransactionId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(t => t.TransactionReference)
            .IsUnique();

        builder.HasIndex(t => t.FixedDepositId);
        builder.HasIndex(t => t.TransactionType);
        builder.HasIndex(t => t.TransactionDate);
        builder.HasIndex(t => new { t.FixedDepositId, t.TransactionType });
        builder.HasIndex(t => new { t.FixedDepositId, t.TransactionDate });
    }
}