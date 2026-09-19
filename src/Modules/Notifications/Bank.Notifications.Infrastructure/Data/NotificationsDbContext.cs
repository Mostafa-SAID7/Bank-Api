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
    internal DbSet<StoredNotificationPreference> Preferences => Set<StoredNotificationPreference>();

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

        modelBuilder.Entity<StoredNotificationPreference>(entity =>
        {
            entity.ToTable("NotificationPreferences");
            entity.HasKey(preference => preference.UserId);
            entity.Property(preference => preference.TransactionAlertThreshold).HasPrecision(18, 2);
            entity.Property(preference => preference.LowBalanceThreshold).HasPrecision(18, 2);
            entity.Property(preference => preference.PreferredChannels).HasMaxLength(500).IsRequired();
            entity.Property(preference => preference.PhoneNumber).HasMaxLength(20);
            entity.Property(preference => preference.Email).HasMaxLength(256);
            entity.Property(preference => preference.Language).HasMaxLength(10).IsRequired();
            entity.Property(preference => preference.TimeZone).HasMaxLength(50).IsRequired();
        });
    }
}
