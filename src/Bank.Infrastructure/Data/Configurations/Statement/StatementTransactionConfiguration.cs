using Bank.Domain.Entities.Account;
using Bank.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations.Statement;

/// <summary>
/// Entity Framework configuration for StatementTransaction entity
/// </summary>
public class StatementTransactionConfiguration : IEntityTypeConfiguration<StatementTransaction>
{
    public void Configure(EntityTypeBuilder<StatementTransaction> builder)
    {
        // Table name
        builder.ToTable("StatementTransactions");

        // Indexes for performance
        builder.HasIndex(st => st.StatementId)
            .HasDatabaseName("IX_StatementTransactions_StatementId");

        builder.HasIndex(st => st.TransactionId)
            .HasDatabaseName("IX_StatementTransactions_TransactionId");

        builder.HasIndex(st => st.TransactionDate)
            .HasDatabaseName("IX_StatementTransactions_TransactionDate");

        builder.HasIndex(st => st.Type)
            .HasDatabaseName("IX_StatementTransactions_Type");

        builder.HasIndex(st => st.Category)
            .HasDatabaseName("IX_StatementTransactions_Category");

        builder.HasIndex(st => new { st.StatementId, st.TransactionDate })
            .HasDatabaseName("IX_StatementTransactions_StatementId_TransactionDate");

        // String property configurations
        builder.Property(st => st.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(st => st.Reference)
            .HasMaxLength(100);

        builder.Property(st => st.Category)
            .HasMaxLength(100);

        builder.Property(st => st.Merchant)
            .HasMaxLength(200);

        // Decimal property configurations
        builder.Property(st => st.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(st => st.Balance)
            .HasPrecision(18, 2)
            .IsRequired();

        // Enum configuration
        builder.Property(st => st.Type)
            .HasConversion<int>()
            .IsRequired();

        // Relationships
        builder.HasOne(st => st.Statement)
            .WithMany(s => s.Transactions)
            .HasForeignKey(st => st.StatementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(st => st.Transaction)
            .WithMany()
            .HasForeignKey(st => st.TransactionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
