using System.Text.Json;
using Bank.Notifications.Application.Notifications;
using Bank.Notifications.Domain.Notifications;
using Microsoft.EntityFrameworkCore;

namespace Bank.Notifications.Infrastructure.Data;

internal sealed class EfNotificationStore(NotificationsDbContext dbContext) : INotificationStore
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
}
