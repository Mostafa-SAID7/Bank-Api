using Microsoft.EntityFrameworkCore;
using Bank.Notifications.Domain;

namespace Bank.Notifications.Infrastructure.Data;

/// <summary>
/// Database boundary owned solely by Notifications. It intentionally has no
/// navigation property or foreign key to Identity's user table.
/// </summary>
public sealed class NotificationsDbContext(DbContextOptions<NotificationsDbContext> options) : DbContext(options)
{
    internal DbSet<StoredNotification> Notifications => Set<StoredNotification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(ModuleMetadata.Schema);

        modelBuilder.Entity<StoredNotification>(entity =>
        {
            entity.ToTable("Notifications");
            entity.HasKey(notification => notification.Id);
            entity.Property(notification => notification.Type).HasMaxLength(100).IsRequired();
            entity.Property(notification => notification.Subject).HasMaxLength(200).IsRequired();
            entity.Property(notification => notification.Message).HasMaxLength(2000).IsRequired();
            entity.Property(notification => notification.IdempotencyKey).HasMaxLength(200).IsRequired();
            entity.Property(notification => notification.Data).HasColumnType("jsonb");
            entity.HasIndex(notification => notification.IdempotencyKey).IsUnique();
            entity.HasIndex(notification => new { notification.UserId, notification.CreatedAt });
            entity.HasIndex(notification => notification.ScheduledAt);
        });
    }
}
