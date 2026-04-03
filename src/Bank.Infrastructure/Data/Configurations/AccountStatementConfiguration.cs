using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for AccountStatement entity
/// </summary>
public class AccountStatementConfiguration : IEntityTypeConfiguration<AccountStatement>
{
    public void Configure(EntityTypeBuilder<AccountStatement> builder)
    {
        builder.ToTable("AccountStatements");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.StatementNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.StatementSequence)
            .IsRequired();

        builder.Property(s => s.OpeningBalance)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(s => s.ClosingBalance)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(s => s.AverageBalance)
            .HasColumnType("decimal(18,2)");

        builder.Property(s => s.MinimumBalance)
            .HasColumnType("decimal(18,2)");

        builder.Property(s => s.MaximumBalance)
            .HasColumnType("decimal(18,2)");

        builder.Property(s => s.TotalDebits)
            .HasColumnType("decimal(18,2)");

        builder.Property(s => s.TotalCredits)
            .HasColumnType("decimal(18,2)");

        builder.Property(s => s.TotalFees)
            .HasColumnType("decimal(18,2)");

        builder.Property(s => s.InterestEarned)
            .HasColumnType("decimal(18,2)");

        builder.Property(s => s.InterestCharged)
            .HasColumnType("decimal(18,2)");

        builder.Property(s => s.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(s => s.Format)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(s => s.DeliveryMethod)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(s => s.FilePath)
            .HasMaxLength(500);

        builder.Property(s => s.FileName)
            .HasMaxLength(255);

        builder.Property(s => s.FileHash)
            .HasMaxLength(100);

        builder.Property(s => s.DeliveryReference)
            .HasMaxLength(100);

        builder.Property(s => s.RequestReason)
            .HasMaxLength(500);

        // Relationships
        builder.HasOne(s => s.Account)
            .WithMany()
            .HasForeignKey(s => s.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.RequestedByUser)
            .WithMany()
            .HasForeignKey(s => s.RequestedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(s => s.Transactions)
            .WithOne(t => t.Statement)
            .HasForeignKey(t => t.StatementId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(s => s.AccountId)
            .HasDatabaseName("IX_AccountStatements_AccountId");

        builder.HasIndex(s => s.StatementNumber)
            .IsUnique()
            .HasDatabaseName("IX_AccountStatements_StatementNumber");

        builder.HasIndex(s => s.StatementDate)
            .HasDatabaseName("IX_AccountStatements_StatementDate");

        builder.HasIndex(s => new { s.AccountId, s.PeriodStartDate, s.PeriodEndDate })
            .HasDatabaseName("IX_AccountStatements_AccountId_Period");

        builder.HasIndex(s => s.Status)
            .HasDatabaseName("IX_AccountStatements_Status");

        builder.HasIndex(s => s.IsDelivered)
            .HasDatabaseName("IX_AccountStatements_IsDelivered");
    }
}

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