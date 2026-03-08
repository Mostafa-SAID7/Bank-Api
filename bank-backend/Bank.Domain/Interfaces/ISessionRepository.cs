using Bank.Domain.Entities;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for Session entity
/// </summary>
public interface ISessionRepository : IRepository<Session>
{
    /// <summary>
    /// Gets a session by session token
    /// </summary>
    Task<Session?> GetBySessionTokenAsync(string sessionToken);

    /// <summary>
    /// Gets a session by refresh token
    /// </summary>
    Task<Session?> GetByRefreshTokenAsync(string refreshToken);

    /// <summary>
    /// Gets all active sessions for a user
    /// </summary>
    Task<List<Session>> GetActiveSessionsByUserIdAsync(Guid userId);

    /// <summary>
    /// Gets all expired sessions
    /// </summary>
    Task<List<Session>> GetExpiredSessionsAsync();

    /// <summary>
    /// Gets all sessions (for statistics)
    /// </summary>
    Task<List<Session>> GetAllSessionsAsync();

    /// <summary>
    /// Gets sessions by IP address
    /// </summary>
    Task<List<Session>> GetSessionsByIpAddressAsync(string ipAddress);

    /// <summary>
    /// Gets admin sessions
    /// </summary>
    Task<List<Session>> GetAdminSessionsAsync();
}