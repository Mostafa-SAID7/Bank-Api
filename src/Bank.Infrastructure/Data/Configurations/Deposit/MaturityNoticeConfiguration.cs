using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for MaturityNotice entity
/// </summary>
public class MaturityNoticeConfiguration : IEntityTypeConfiguration<MaturityNotice>
{
    public void Configure(EntityTypeBuilder<MaturityNotice> builder)
    {
        builder.ToTable("MaturityNotices");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.NoticeNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(n => n.NoticeType)
            .HasConversion<int>();

        builder.Property(n => n.Status)
            .HasConversion<int>();

        builder.Property(n => n.Subject)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(n => n.Content)
            .HasMaxLength(4000);

        builder.Property(n => n.TemplateUsed)
            .HasMaxLength(100);

        builder.Property(n => n.DeliveryChannel)
            .HasConversion<int>();

        builder.Property(n => n.DeliveryAddress)
            .HasMaxLength(500);

        builder.Property(n => n.DeliveryReference)
            .HasMaxLength(100);

        builder.Property(n => n.CustomerChoice)
            .HasConversion<int>();

        builder.Property(n => n.CustomerInstructions)
            .HasMaxLength(1000);

        builder.Property(n => n.ProcessingNotes)
            .HasMaxLength(1000);

        // Relationships
        builder.HasOne(n => n.FixedDeposit)
            .WithMany(d => d.MaturityNotices)
            .HasForeignKey(n => n.FixedDepositId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(n => n.GeneratedByUser)
            .WithMany()
            .HasForeignKey(n => n.GeneratedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(n => n.NoticeNumber)
            .IsUnique();

        builder.HasIndex(n => n.FixedDepositId);
        builder.HasIndex(n => n.NoticeType);
        builder.HasIndex(n => n.Status);
        builder.HasIndex(n => n.MaturityDate);
        builder.HasIndex(n => new { n.Status, n.NoticeDate });
    }
}

