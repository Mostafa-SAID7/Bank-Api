using Bank.Domain.Enums;

namespace Bank.Application.DTOs;

/// <summary>
/// Result of a two-factor authentication token generation request
/// </summary>
public class TwoFactorTokenResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? TokenId { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

/// <summary>
/// Result of a two-factor authentication token verification
/// </summary>
public class TwoFactorVerificationResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public bool RequiresAdditionalVerification { get; set; }
}

/// <summary>
/// Result of two-factor authentication setup operations
/// </summary>
public class TwoFactorSetupResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? SecretKey { get; set; }
    public string? QrCodeUrl { get; set; }
    public List<string>? BackupCodes { get; set; }
}

/// <summary>
/// Current status and configuration of two-factor authentication for a user
/// </summary>
public class TwoFactorStatusResult
{
    public bool IsEnabled { get; set; }
    public TwoFactorStatus Status { get; set; }
    public List<TwoFactorMethod> EnabledMethods { get; set; } = new();
    public DateTime? SetupDate { get; set; }
    public DateTime? LastUsed { get; set; }
}