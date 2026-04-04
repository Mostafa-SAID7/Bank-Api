using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for DepositCertificate entity
/// </summary>
public class DepositCertificateConfiguration : IEntityTypeConfiguration<DepositCertificate>
{
    public void Configure(EntityTypeBuilder<DepositCertificate> builder)
    {
        builder.ToTable("DepositCertificates");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CertificateNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Status)
            .HasConversion<int>();

        builder.Property(c => c.DeliveryMethod)
            .HasMaxLength(50);

        builder.Property(c => c.DeliveryAddress)
            .HasMaxLength(500);

        builder.Property(c => c.DeliveryReference)
            .HasMaxLength(100);

        builder.Property(c => c.CertificateTemplate)
            .HasMaxLength(100);

        builder.Property(c => c.CertificateContent)
            .HasMaxLength(4000);

        builder.Property(c => c.PdfFileName)
            .HasMaxLength(255);

        builder.Property(c => c.DigitalSignature)
            .HasMaxLength(1000);

        builder.Property(c => c.SecurityHash)
            .HasMaxLength(500);

        builder.Property(c => c.ReplacementReason)
            .HasMaxLength(500);

        builder.Property(c => c.ProcessingNotes)
            .HasMaxLength(1000);

        // Relationships
        builder.HasOne(c => c.FixedDeposit)
            .WithMany(d => d.Certificates)
            .HasForeignKey(c => c.FixedDepositId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.ReplacedCertificate)
            .WithMany()
            .HasForeignKey(c => c.ReplacedCertificateId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.GeneratedByUser)
            .WithMany()
            .HasForeignKey(c => c.GeneratedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.IssuedByUser)
            .WithMany()
            .HasForeignKey(c => c.IssuedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.ReplacedByUser)
            .WithMany()
            .HasForeignKey(c => c.ReplacedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(c => c.CertificateNumber)
            .IsUnique();

        builder.HasIndex(c => c.FixedDepositId);
        builder.HasIndex(c => c.Status);
        builder.HasIndex(c => c.IssueDate);
    }
}

