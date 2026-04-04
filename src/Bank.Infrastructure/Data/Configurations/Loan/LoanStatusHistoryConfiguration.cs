using Bank.Domain.Entities;
using Loan = Bank.Domain.Entities.Loan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for LoanStatusHistory entity
/// </summary>
public class LoanStatusHistoryConfiguration : IEntityTypeConfiguration<LoanStatusHistory>
{
    public void Configure(EntityTypeBuilder<LoanStatusHistory> builder)
    {
        builder.ToTable("LoanStatusHistories");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Reason)
            .HasMaxLength(500);

        builder.Property(h => h.Notes)
            .HasMaxLength(1000);

        builder.Property(h => h.SystemReference)
            .HasMaxLength(100);

        // Relationships
        builder.HasOne(h => h.Loan)
            .WithMany(l => l.StatusHistory)
            .HasForeignKey(h => h.LoanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(h => h.ChangedByUser)
            .WithMany()
            .HasForeignKey(h => h.ChangedBy)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(h => h.LoanId);
        builder.HasIndex(h => h.StatusChangeDate);
        builder.HasIndex(h => h.FromStatus);
        builder.HasIndex(h => h.ToStatus);
    }
}


