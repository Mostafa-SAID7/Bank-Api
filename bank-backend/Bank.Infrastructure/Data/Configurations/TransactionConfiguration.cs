using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Transaction entity
/// </summary>
public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasOne(t => t.FromAccount)
            .WithMany(a => a.SentTransactions)
            .HasForeignKey(t => t.FromAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.ToAccount)
            .WithMany(a => a.ReceivedTransactions)
            .HasForeignKey(t => t.ToAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(t => t.Amount)
            .HasPrecision(18, 2);
    }
}