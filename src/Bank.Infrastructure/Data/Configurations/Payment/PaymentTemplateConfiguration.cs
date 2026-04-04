using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for PaymentTemplate entity
/// </summary>
public class PaymentTemplateConfiguration : IEntityTypeConfiguration<PaymentTemplate>
{
    public void Configure(EntityTypeBuilder<PaymentTemplate> builder)
    {
        builder.HasOne(pt => pt.FromAccount)
            .WithMany()
            .HasForeignKey(pt => pt.FromAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pt => pt.ToAccount)
            .WithMany()
            .HasForeignKey(pt => pt.ToAccountId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(pt => pt.CreatedByUser)
            .WithMany()
            .HasForeignKey(pt => pt.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(pt => pt.Amount)
            .HasPrecision(18, 2);

        // Indexes for performance
        builder.HasIndex(pt => new { pt.CreatedByUserId, pt.IsActive })
            .HasDatabaseName("IX_PaymentTemplates_CreatedByUserId_IsActive");

        builder.HasIndex(pt => new { pt.CreatedByUserId, pt.Category })
            .HasDatabaseName("IX_PaymentTemplates_CreatedByUserId_Category");

        builder.HasIndex(pt => new { pt.CreatedByUserId, pt.UsageCount })
            .HasDatabaseName("IX_PaymentTemplates_CreatedByUserId_UsageCount");

        builder.HasIndex(pt => new { pt.CreatedByUserId, pt.LastUsedDate })
            .HasDatabaseName("IX_PaymentTemplates_CreatedByUserId_LastUsedDate");

        // String property configurations
        builder.Property(pt => pt.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(pt => pt.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(pt => pt.BeneficiaryName)
            .HasMaxLength(200);

        builder.Property(pt => pt.BeneficiaryAccountNumber)
            .HasMaxLength(50);

        builder.Property(pt => pt.BeneficiaryBankCode)
            .HasMaxLength(20);

        builder.Property(pt => pt.Reference)
            .HasMaxLength(100);

        builder.Property(pt => pt.Notes)
            .HasMaxLength(1000);

        builder.Property(pt => pt.Tags)
            .HasMaxLength(1000);
    }
}

