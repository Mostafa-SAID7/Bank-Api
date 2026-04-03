using Bank.Application.DTOs;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service for managing notifications
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Send a notification to a user
    /// </summary>
    Task<NotificationResponse> SendNotificationAsync(SendNotificationRequest request);
    
    /// <summary>
    /// Send transaction alert notification
    /// </summary>
    Task<NotificationResponse> SendTransactionAlertAsync(TransactionAlertRequest request);
    
    /// <summary>
    /// Send security alert notification
    /// </summary>
    Task<NotificationResponse> SendSecurityAlertAsync(SecurityAlertRequest request);
    
    /// <summary>
    /// Send bulk notifications to multiple users
    /// </summary>
    Task<List<NotificationResponse>> SendBulkNotificationAsync(BulkNotificationRequest request);
    
    /// <summary>
    /// Get notification history for a user
    /// </summary>
    Task<List<NotificationHistoryItem>> GetNotificationHistoryAsync(string userId, int page = 1, int pageSize = 20);
    
    /// <summary>
    /// Mark notification as read
    /// </summary>
    Task<bool> MarkAsReadAsync(string notificationId, string userId);
    
    /// <summary>
    /// Get unread notification count for a user
    /// </summary>
    Task<int> GetUnreadCountAsync(string userId);
    
    /// <summary>
    /// Update notification preferences for a user
    /// </summary>
    Task<bool> UpdatePreferencesAsync(NotificationPreferencesRequest request);
    
    /// <summary>
    /// Get notification preferences for a user
    /// </summary>
    Task<NotificationPreferencesRequest?> GetPreferencesAsync(string userId);
    
    /// <summary>
    /// Send low balance alert
    /// </summary>
    Task<NotificationResponse> SendLowBalanceAlertAsync(string userId, string accountId, decimal currentBalance, decimal threshold);
    
    /// <summary>
    /// Send card alert notification
    /// </summary>
    Task<NotificationResponse> SendCardAlertAsync(string userId, string cardId, string alertType, string message);
    
    /// <summary>
    /// Send payment reminder notification
    /// </summary>
    Task<NotificationResponse> SendPaymentReminderAsync(string userId, string paymentType, decimal amount, DateTime dueDate);
    
    /// <summary>
    /// Process scheduled notifications
    /// </summary>
    Task ProcessScheduledNotificationsAsync();
    
    /// <summary>
    /// Retry failed notifications
    /// </summary>
    Task RetryFailedNotificationsAsync();
}