using Bank.Domain.Entities;
using Loan = Bank.Domain.Entities.Loan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Loan entity
/// </summary>
public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.ToTable("Loans");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.LoanNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(l => l.LoanNumber)
            .IsUnique();

        builder.Property(l => l.PrincipalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(l => l.RequestedAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(l => l.InterestRate)
            .HasColumnType("decimal(5,4)")
            .IsRequired();

        builder.Property(l => l.OutstandingBalance)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(l => l.TotalInterestPaid)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(l => l.TotalPrincipalPaid)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(l => l.MonthlyPaymentAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(l => l.Purpose)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(l => l.RejectionReason)
            .HasMaxLength(1000);

        // Relationships
        builder.HasOne(l => l.Customer)
            .WithMany()
            .HasForeignKey(l => l.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.ApprovedByUser)
            .WithMany()
            .HasForeignKey(l => l.ApprovedBy)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(l => l.Payments)
            .WithOne(p => p.Loan)
            .HasForeignKey(p => p.LoanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(l => l.Documents)
            .WithOne(d => d.Loan)
            .HasForeignKey(d => d.LoanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(l => l.StatusHistory)
            .WithOne(h => h.Loan)
            .HasForeignKey(h => h.LoanId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(l => l.CustomerId);
        builder.HasIndex(l => l.Status);
        builder.HasIndex(l => l.ApplicationDate);
        builder.HasIndex(l => l.Type);
        builder.HasIndex(l => l.NextPaymentDueDate);
    }
}


