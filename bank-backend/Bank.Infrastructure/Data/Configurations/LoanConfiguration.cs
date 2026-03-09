using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for loan-related entities
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

public class LoanDocumentConfiguration : IEntityTypeConfiguration<LoanDocument>
{
    public void Configure(EntityTypeBuilder<LoanDocument> builder)
    {
        builder.ToTable("LoanDocuments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.DocumentName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(d => d.FilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(d => d.ContentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Description)
            .HasMaxLength(1000);

        builder.Property(d => d.VerificationNotes)
            .HasMaxLength(1000);

        // Relationships
        builder.HasOne(d => d.Loan)
            .WithMany(l => l.Documents)
            .HasForeignKey(d => d.LoanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.VerifiedByUser)
            .WithMany()
            .HasForeignKey(d => d.VerifiedBy)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(d => d.LoanId);
        builder.HasIndex(d => d.DocumentType);
        builder.HasIndex(d => d.IsVerified);
    }
}

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