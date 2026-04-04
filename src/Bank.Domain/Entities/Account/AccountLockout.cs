using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Account lockout entity for tracking failed login attempts and account security measures
/// </summary>
public class AccountLockout : BaseEntity
{
    public Guid UserId { get; private set; }
    public int FailedAttempts { get; private set; }
    public DateTime? LockedUntil { get; private set; }
    public AccountLockoutReason? LockoutReason { get; private set; }
    public string? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }
    public DateTime? LastFailedAttempt { get; private set; }
    public DateTime? LastSuccessfulLogin { get; private set; }
    public bool IsCurrentlyLocked { get; private set; }
    public string? LockoutNotes { get; private set; }
    public Guid? LockedByUserId { get; private set; }

    // Navigation properties
    public User User { get; private set; } = null!;
    public User? LockedByUser { get; private set; }

    // Private constructor for EF Core
    private AccountLockout() { }

    public AccountLockout(Guid userId)
    {
        UserId = userId;
        FailedAttempts = 0;
        IsCurrentlyLocked = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void RecordFailedAttempt(string? ipAddress = null, string? userAgent = null)
    {
        FailedAttempts++;
        LastFailedAttempt = DateTime.UtcNow;
        IpAddress = ipAddress;
        UserAgent = userAgent;
    }

    public void LockAccount(AccountLockoutReason reason, TimeSpan lockoutDuration, string? notes = null, Guid? lockedByUserId = null)
    {
        IsCurrentlyLocked = true;
        LockedUntil = DateTime.UtcNow.Add(lockoutDuration);
        LockoutReason = reason;
        LockoutNotes = notes;
        LockedByUserId = lockedByUserId;
    }

    public void LockAccountPermanently(AccountLockoutReason reason, string? notes = null, Guid? lockedByUserId = null)
    {
        IsCurrentlyLocked = true;
        LockedUntil = null; // Permanent lockout
        LockoutReason = reason;
        LockoutNotes = notes;
        LockedByUserId = lockedByUserId;
    }

    public void UnlockAccount(Guid? unlockedByUserId = null)
    {
        IsCurrentlyLocked = false;
        LockedUntil = null;
        LockoutReason = null;
        LockoutNotes = null;
        LockedByUserId = unlockedByUserId;
        FailedAttempts = 0; // Reset failed attempts on unlock
    }

    public void RecordSuccessfulLogin()
    {
        LastSuccessfulLogin = DateTime.UtcNow;
        FailedAttempts = 0; // Reset failed attempts on successful login
        
        // Auto-unlock if it was a temporary lockout that has expired
        if (IsCurrentlyLocked && LockedUntil.HasValue && DateTime.UtcNow > LockedUntil.Value)
        {
            UnlockAccount();
        }
    }

    public bool ShouldLockAccount(int maxFailedAttempts)
    {
        return !IsCurrentlyLocked && FailedAttempts >= maxFailedAttempts;
    }

    public bool IsLockoutExpired()
    {
        return IsCurrentlyLocked && LockedUntil.HasValue && DateTime.UtcNow > LockedUntil.Value;
    }

    public TimeSpan? GetRemainingLockoutTime()
    {
        if (!IsCurrentlyLocked || !LockedUntil.HasValue)
            return null;

        var remaining = LockedUntil.Value - DateTime.UtcNow;
        return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
    }
}
