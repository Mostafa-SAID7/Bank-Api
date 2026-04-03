using Bank.Domain.Entities.Shared;
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
        builder.ToTable("StatementTransactions");

        builder.HasKey(st => st.Id);

        builder.Property(st => st.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(st => st.Reference)
            .HasMaxLength(100);

        builder.Property(st => st.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(st => st.RunningBalance)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(st => st.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(st => st.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(st => st.Category)
            .HasMaxLength(100);

        builder.Property(st => st.Memo)
            .HasMaxLength(500);

        // Relationships
        builder.HasOne(st => st.Statement)
            .WithMany(s => s.Transactions)
            .HasForeignKey(st => st.StatementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(st => st.Transaction)
            .WithMany()
            .HasForeignKey(st => st.TransactionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(st => st.StatementId)
            .HasDatabaseName("IX_StatementTransactions_StatementId");

        builder.HasIndex(st => st.TransactionId)
            .HasDatabaseName("IX_StatementTransactions_TransactionId");

        builder.HasIndex(st => st.TransactionDate)
            .HasDatabaseName("IX_StatementTransactions_TransactionDate");

        builder.HasIndex(st => st.Category)
            .HasDatabaseName("IX_StatementTransactions_Category");
    }
}
