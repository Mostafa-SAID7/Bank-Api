using Bank.Domain.Entities.Loan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations.Loan;

/// <summary>
/// Entity Framework configuration for LoanDocument entity
/// </summary>
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
