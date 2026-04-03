using Bank.Domain.Entities.Loan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations.Loan;

/// <summary>
/// Entity Framework configuration for LoanPayment entity
/// </summary>
public class LoanPaymentConfiguration : IEntityTypeConfiguration<LoanPayment>
{
    public void Configure(EntityTypeBuilder<LoanPayment> builder)
    {
        builder.ToTable("LoanPayments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.PaymentAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.PrincipalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.InterestAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.LateFeeAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.OutstandingBalanceAfterPayment)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.TransactionReference)
            .HasMaxLength(100);

        builder.Property(p => p.PaymentMethod)
            .HasMaxLength(50);

        builder.Property(p => p.Notes)
            .HasMaxLength(1000);

        // Relationships
        builder.HasOne(p => p.Loan)
            .WithMany(l => l.Payments)
            .HasForeignKey(p => p.LoanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.ProcessedByUser)
            .WithMany()
            .HasForeignKey(p => p.ProcessedBy)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(p => p.LoanId);
        builder.HasIndex(p => p.PaymentDate);
        builder.HasIndex(p => p.DueDate);
        builder.HasIndex(p => p.Status);
    }
}
