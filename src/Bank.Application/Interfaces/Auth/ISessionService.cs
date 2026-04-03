using Bank.Application.DTOs;
using Bank.Domain.Entities;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for managing user sessions with timeout and concurrent session limits
/// </summary>
public interface ISessionService
{
    /// <summary>
    /// Creates a new session for a user
    /// </summary>
    Task<SessionResult> CreateSessionAsync(Guid userId, string ipAddress, string userAgent, string? deviceFingerprint = null, bool isAdminSession = false);

    /// <summary>
    /// Validates and retrieves a session by token
    /// </summary>
    Task<Session?> GetSessionAsync(string sessionToken);

    /// <summary>
    /// Updates session activity timestamp
    /// </summary>
    Task UpdateSessionActivityAsync(string sessionToken);

    /// <summary>
    /// Terminates a specific session
    /// </summary>
    Task TerminateSessionAsync(string sessionToken, string reason);

    /// <summary>
    /// Terminates all sessions for a user
    /// </summary>
    Task TerminateAllUserSessionsAsync(Guid userId, string reason, string? excludeSessionToken = null);

    /// <summary>
    /// Gets all active sessions for a user
    /// </summary>
    Task<List<Session>> GetUserActiveSessionsAsync(Guid userId);

    /// <summary>
    /// Refreshes session tokens
    /// </summary>
    Task<SessionResult> RefreshSessionAsync(string refreshToken);

    /// <summary>
    /// Cleans up expired sessions
    /// </summary>
    Task CleanupExpiredSessionsAsync();

    /// <summary>
    /// Enforces concurrent session limits for a user
    /// </summary>
    Task EnforceConcurrentSessionLimitsAsync(Guid userId, int maxConcurrentSessions);

    /// <summary>
    /// Validates if a session is valid and not expired
    /// </summary>
    Task<bool> IsSessionValidAsync(string sessionToken);

    /// <summary>
    /// Gets session statistics for monitoring
    /// </summary>
    Task<SessionStatistics> GetSessionStatisticsAsync();
}