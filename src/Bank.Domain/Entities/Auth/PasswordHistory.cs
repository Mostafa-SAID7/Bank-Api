using Bank.Domain.Common;

namespace Bank.Domain.Entities;

/// <summary>
/// Password history entity for tracking previous passwords to prevent reuse
/// </summary>
public class PasswordHistory : BaseEntity
{
    public Guid UserId { get; private set; }
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTime PasswordSetAt { get; private set; }
    public string? PasswordSalt { get; private set; }
    public bool IsCurrentPassword { get; private set; }

    // Navigation properties
    public User User { get; private set; } = null!;

    // Private constructor for EF Core
    private PasswordHistory() { }

    public PasswordHistory(Guid userId, string passwordHash, string? passwordSalt = null, bool isCurrentPassword = false)
    {
        UserId = userId;
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        PasswordSalt = passwordSalt;
        PasswordSetAt = DateTime.UtcNow;
        IsCurrentPassword = isCurrentPassword;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsOldPassword()
    {
        IsCurrentPassword = false;
    }

    public void MarkAsCurrentPassword()
    {
        IsCurrentPassword = true;
    }
}