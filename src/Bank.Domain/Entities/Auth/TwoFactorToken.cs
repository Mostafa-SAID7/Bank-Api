using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents a two-factor authentication token for enhanced security
/// </summary>
public class TwoFactorToken : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public string Token { get; set; } = string.Empty;
    public TwoFactorMethod Method { get; set; }
    public string Destination { get; set; } = string.Empty; // Phone number or email
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; } = false;
    public DateTime? UsedAt { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    
    public bool IsValid => !IsUsed && DateTime.UtcNow < ExpiresAt && !IsDeleted;
    
    public void MarkAsUsed(string? ipAddress = null, string? userAgent = null)
    {
        IsUsed = true;
        UsedAt = DateTime.UtcNow;
        IpAddress = ipAddress;
        UserAgent = userAgent;
    }
}