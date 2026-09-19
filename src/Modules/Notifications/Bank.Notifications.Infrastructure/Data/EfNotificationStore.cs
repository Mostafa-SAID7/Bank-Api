using System.Text.Json;
using Bank.Contracts.Notifications;
using Bank.Notifications.Application.Notifications;
using Bank.Notifications.Domain.Notifications;
using Microsoft.EntityFrameworkCore;

namespace Bank.Notifications.Infrastructure.Data;

internal sealed class EfNotificationStore(NotificationsDbContext dbContext) : INotificationStore, INotificationReadStore
{
    public async Task<NotificationRecord?> FindByIdempotencyKeyAsync(string idempotencyKey, CancellationToken cancellationToken = default)
    {
        var stored = await dbContext.Notifications.AsNoTracking()
            .SingleOrDefaultAsync(notification => notification.IdempotencyKey == idempotencyKey, cancellationToken);

        return stored is null ? null : ToDomain(stored);
    }

    public async Task<NotificationRecord> AddIfAbsentAsync(NotificationRecord notification, CancellationToken cancellationToken = default)
    {
        try
        {
            dbContext.Notifications.Add(ToStored(notification));
            await dbContext.SaveChangesAsync(cancellationToken);
            return notification;
        }
        catch (DbUpdateException exception)
        {
            var existing = await FindByIdempotencyKeyAsync(notification.IdempotencyKey, cancellationToken);
            if (existing is not null)
            {
                return existing;
            }

            throw new InvalidOperationException(
                "Notifications persistence failed without a matching idempotency record.",
                exception);
        }
    }

    public async Task<IReadOnlyList<NotificationHistoryEntry>> GetHistoryAsync(
        Guid userId, int page, int pageSize, CancellationToken cancellationToken = default) =>
        await dbContext.Notifications.AsNoTracking()
            .Where(notification => notification.UserId == userId)
            .OrderByDescending(notification => notification.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(notification => new NotificationHistoryEntry(
                notification.Id, notification.Type, notification.Subject, notification.Message,
                (NotificationChannel)notification.Channel, (NotificationDeliveryStatus)notification.Status,
                notification.CreatedAt, notification.SentAt, notification.ReadAt, notification.ErrorMessage))
            .ToListAsync(cancellationToken);

    public Task<int> GetUnreadCountAsync(Guid userId, CancellationToken cancellationToken = default) =>
        dbContext.Notifications.CountAsync(
            notification => notification.UserId == userId && notification.Status != (int)NotificationDeliveryStatus.Read,
            cancellationToken);

    public async Task<bool> MarkAsReadAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken = default)
    {
        var notification = await dbContext.Notifications.SingleOrDefaultAsync(
            candidate => candidate.Id == notificationId && candidate.UserId == userId,
            cancellationToken);
        if (notification is null)
        {
            return false;
        }

        notification.Status = (int)NotificationDeliveryStatus.Read;
        notification.ReadAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<NotificationPreferences?> GetPreferencesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var preference = await dbContext.Preferences.AsNoTracking()
            .SingleOrDefaultAsync(candidate => candidate.UserId == userId, cancellationToken);
        return preference is null ? null : ToContract(preference);
    }

    public async Task SavePreferencesAsync(Guid userId, NotificationPreferences preferences, CancellationToken cancellationToken = default)
    {
        var stored = await dbContext.Preferences.SingleOrDefaultAsync(candidate => candidate.UserId == userId, cancellationToken);
        if (stored is null)
        {
            stored = new StoredNotificationPreference { UserId = userId };
            dbContext.Preferences.Add(stored);
        }

        stored.TransactionAlerts = preferences.TransactionAlerts;
        stored.SecurityAlerts = preferences.SecurityAlerts;
        stored.LowBalanceAlerts = preferences.LowBalanceAlerts;
        stored.PaymentReminders = preferences.PaymentReminders;
        stored.MarketingNotifications = preferences.MarketingNotifications;
        stored.TransactionAlertThreshold = preferences.TransactionAlertThreshold;
        stored.LowBalanceThreshold = preferences.LowBalanceThreshold;
        stored.PreferredChannels = JsonSerializer.Serialize(preferences.PreferredChannels.Select(channel => (int)channel));
        stored.PhoneNumber = preferences.PhoneNumber;
        stored.Email = preferences.Email;
        stored.Language = preferences.Language;
        stored.TimeZone = preferences.TimeZone;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static StoredNotification ToStored(NotificationRecord notification) => new()
    {
        Id = notification.Id,
        UserId = notification.UserId,
        Type = notification.Type,
        Subject = notification.Subject,
        Message = notification.Message,
        Channel = (int)notification.Channel,
        Priority = (int)notification.Priority,
        IdempotencyKey = notification.IdempotencyKey,
        CreatedAt = notification.CreatedAt,
        ScheduledAt = notification.ScheduledAt,
        Data = notification.Data is null ? null : JsonSerializer.Serialize(notification.Data)
    };

    private static NotificationRecord ToDomain(StoredNotification notification) => new(
        notification.Id, notification.UserId, notification.Type, notification.Subject,
        notification.Message, (Bank.Contracts.Notifications.NotificationChannel)notification.Channel,
        (Bank.Contracts.Notifications.NotificationPriority)notification.Priority,
        notification.IdempotencyKey, notification.CreatedAt, notification.ScheduledAt,
        notification.Data is null ? null : JsonSerializer.Deserialize<Dictionary<string, string>>(notification.Data));

    private static NotificationPreferences ToContract(StoredNotificationPreference preference)
    {
        var channels = JsonSerializer.Deserialize<List<int>>(preference.PreferredChannels)
            ?.Select(value => (NotificationChannel)value).ToArray()
            ?? [NotificationChannel.InApp];
        return new NotificationPreferences(
            preference.TransactionAlerts, preference.SecurityAlerts, preference.LowBalanceAlerts,
            preference.PaymentReminders, preference.MarketingNotifications,
            preference.TransactionAlertThreshold, preference.LowBalanceThreshold, channels,
            preference.PhoneNumber, preference.Email, preference.Language, preference.TimeZone);
    }
}
