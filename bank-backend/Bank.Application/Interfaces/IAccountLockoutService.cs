using Bank.Application.DTOs;
using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for managing account lockouts and failed login attempts
/// </summary>
public interface IAccountLockoutService
{
    /// <summary>
    /// Records a failed login attempt for a user
    /// </summary>
    Task<LockoutResult> RecordFailedLoginAttemptAsync(Guid userId, string? ipAddress = null, string? userAgent = null);

    /// <summary>
    /// Records a successful login and resets failed attempts
    /// </summary>
    Task RecordSuccessfulLoginAsync(Guid userId);

    /// <summary>
    /// Checks if an account is currently locked
    /// </summary>
    Task<bool> IsAccountLockedAsync(Guid userId);

    /// <summary>
    /// Gets the lockout status for a user
    /// </summary>
    Task<AccountLockout?> GetLockoutStatusAsync(Guid userId);

    /// <summary>
    /// Manually locks an account
    /// </summary>
    Task<bool> LockAccountAsync(Guid userId, AccountLockoutReason reason, TimeSpan? lockoutDuration = null, string? notes = null, Guid? lockedByUserId = null);

    /// <summary>
    /// Manually unlocks an account
    /// </summary>
    Task<bool> UnlockAccountAsync(Guid userId, Guid? unlockedByUserId = null);

    /// <summary>
    /// Gets the remaining lockout time for a user
    /// </summary>
    Task<TimeSpan?> GetRemainingLockoutTimeAsync(Guid userId);

    /// <summary>
    /// Gets failed login attempt count for a user
    /// </summary>
    Task<int> GetFailedAttemptCountAsync(Guid userId);

    /// <summary>
    /// Cleans up expired lockouts
    /// </summary>
    Task CleanupExpiredLockoutsAsync();

    /// <summary>
    /// Gets lockout statistics for monitoring
    /// </summary>
    Task<LockoutStatistics> GetLockoutStatisticsAsync();

    /// <summary>
    /// Gets all currently locked accounts
    /// </summary>
    Task<List<AccountLockout>> GetLockedAccountsAsync();

    /// <summary>
    /// Gets lockout history for a user
    /// </summary>
    Task<List<AccountLockout>> GetLockoutHistoryAsync(Guid userId);

    /// <summary>
    /// Checks if an account should be locked based on failed attempts
    /// </summary>
    Task<bool> ShouldLockAccountAsync(Guid userId);
}