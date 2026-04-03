using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations.Shared;

/// <summary>
/// Entity Framework configuration for audit-related entities
/// </summary>
public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        // AuditLog configuration - immutable, no soft delete
        builder.HasOne(al => al.User)
            .WithMany()
            .HasForeignKey(al => al.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes for performance
        builder.HasIndex(al => new { al.UserId, al.CreatedAt })
            .HasDatabaseName("IX_AuditLogs_UserId_CreatedAt");

        builder.HasIndex(al => new { al.EntityType, al.EntityId })
            .HasDatabaseName("IX_AuditLogs_EntityType_EntityId");

        builder.HasIndex(al => al.EventType)
            .HasDatabaseName("IX_AuditLogs_EventType");

        builder.HasIndex(al => al.Action)
            .HasDatabaseName("IX_AuditLogs_Action");

        builder.HasIndex(al => al.IpAddress)
            .HasDatabaseName("IX_AuditLogs_IpAddress");

        builder.HasIndex(al => al.CreatedAt)
            .HasDatabaseName("IX_AuditLogs_CreatedAt");

        // Property configurations
        builder.Property(al => al.Action)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(al => al.EntityType)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(al => al.EntityId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(al => al.IpAddress)
            .HasMaxLength(45);

        builder.Property(al => al.UserAgent)
            .HasMaxLength(500);
    }
}