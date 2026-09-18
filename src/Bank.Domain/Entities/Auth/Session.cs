using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// User session entity for tracking active sessions with timeout and concurrent session management
/// </summary>
public class Session : BaseEntity
{

    public Guid UserId { get; private set; }
    public string SessionToken { get; private set; } = string.Empty;
    public string? RefreshTokenHash { get; private set; }
    public string? ReplacedByToken { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RefreshTokenExpiresAt { get; private set; }
    public SessionStatus Status { get; private set; }
    public string IpAddress { get; private set; } = string.Empty;
    public string UserAgent { get; private set; } = string.Empty;
    public string? DeviceFingerprint { get; private set; }
    public DateTime LastActivityAt { get; private set; }
    public DateTime? TerminatedAt { get; private set; }
    public string? TerminationReason { get; private set; }
    public bool IsAdminSession { get; private set; }
    
    public bool IsRevoked => TerminatedAt.HasValue || Status == SessionStatus.Terminated || Status == SessionStatus.Locked;


    // Navigation properties
    public User User { get; private set; } = null!;

    // Private constructor for EF Core
    private Session() { }

    public Session(
        Guid userId,
        string sessionToken,
        string? refreshTokenHash,
        TimeSpan sessionTimeout,
        TimeSpan? refreshTokenTimeout,
        string ipAddress,
        string userAgent,
        string? deviceFingerprint = null,
        bool isAdminSession = false)
    {
        UserId = userId;
        SessionToken = sessionToken ?? throw new ArgumentNullException(nameof(sessionToken));
        RefreshTokenHash = refreshTokenHash;
        ExpiresAt = DateTime.UtcNow.Add(sessionTimeout);
        RefreshTokenExpiresAt = refreshTokenTimeout.HasValue ? DateTime.UtcNow.Add(refreshTokenTimeout.Value) : null;
        Status = SessionStatus.Active;
        IpAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
        UserAgent = userAgent ?? throw new ArgumentNullException(nameof(userAgent));
        DeviceFingerprint = deviceFingerprint;
        LastActivityAt = DateTime.UtcNow;
        IsAdminSession = isAdminSession;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateActivity()
    {
        if (Status == SessionStatus.Active)
        {
            LastActivityAt = DateTime.UtcNow;
        }
    }

    public void ExtendSession(TimeSpan additionalTime)
    {
        if (Status == SessionStatus.Active)
        {
            ExpiresAt = DateTime.UtcNow.Add(additionalTime);
            LastActivityAt = DateTime.UtcNow;
        }
    }

    public void Terminate(string reason)
    {
        Status = SessionStatus.Terminated;
        TerminatedAt = DateTime.UtcNow;
        TerminationReason = reason;
    }

    public void Lock(string reason)
    {
        Status = SessionStatus.Locked;
        TerminationReason = reason;
    }

    public bool IsExpired()
    {
        return DateTime.UtcNow > ExpiresAt;
    }

    public bool IsRefreshTokenExpired()
    {
        return RefreshTokenExpiresAt.HasValue && DateTime.UtcNow > RefreshTokenExpiresAt.Value;
    }

    public bool IsValid()
    {
        return Status == SessionStatus.Active && !IsExpired();
    }

    public void RefreshTokens(string newSessionToken, string? newRefreshTokenHash, string replacementToken, TimeSpan sessionTimeout, TimeSpan? refreshTokenTimeout)
    {
        if (Status == SessionStatus.Active)
        {
            ReplacedByToken = replacementToken;
            TerminatedAt = DateTime.UtcNow;
            TerminationReason = "Rotated";
            Status = SessionStatus.Terminated;
        }
    }

    public void RevokeFamily(string reason)
    {
        Terminate(reason);
        ReplacedByToken = null;
    }
}
