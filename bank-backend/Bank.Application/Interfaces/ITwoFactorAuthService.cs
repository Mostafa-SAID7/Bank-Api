using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service for managing two-factor authentication
/// </summary>
public interface ITwoFactorAuthService
{
    /// <summary>
    /// Generate and send a 2FA token to the user
    /// </summary>
    Task<TwoFactorTokenResult> GenerateTokenAsync(Guid userId, TwoFactorMethod method, string? destination = null);
    
    /// <summary>
    /// Verify a 2FA token provided by the user
    /// </summary>
    Task<TwoFactorVerificationResult> VerifyTokenAsync(Guid userId, string token, string? ipAddress = null, string? userAgent = null);
    
    /// <summary>
    /// Setup 2FA for a user with authenticator app
    /// </summary>
    Task<TwoFactorSetupResult> SetupAuthenticatorAsync(Guid userId);
    
    /// <summary>
    /// Complete 2FA setup by verifying the initial token
    /// </summary>
    Task<TwoFactorSetupResult> CompleteSetupAsync(Guid userId, string verificationToken);
    
    /// <summary>
    /// Disable 2FA for a user
    /// </summary>
    Task<bool> DisableTwoFactorAsync(Guid userId);
    
    /// <summary>
    /// Generate backup codes for 2FA recovery
    /// </summary>
    Task<List<string>> GenerateBackupCodesAsync(Guid userId);
    
    /// <summary>
    /// Verify a backup code
    /// </summary>
    Task<bool> VerifyBackupCodeAsync(Guid userId, string backupCode);
    
    /// <summary>
    /// Check if user has 2FA enabled
    /// </summary>
    Task<bool> IsTwoFactorEnabledAsync(Guid userId);
    
    /// <summary>
    /// Get user's 2FA status and methods
    /// </summary>
    Task<TwoFactorStatusResult> GetTwoFactorStatusAsync(Guid userId);
}

public class TwoFactorTokenResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? TokenId { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

public class TwoFactorVerificationResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public bool RequiresAdditionalVerification { get; set; }
}

public class TwoFactorSetupResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? SecretKey { get; set; }
    public string? QrCodeUrl { get; set; }
    public List<string>? BackupCodes { get; set; }
}

public class TwoFactorStatusResult
{
    public bool IsEnabled { get; set; }
    public TwoFactorStatus Status { get; set; }
    public List<TwoFactorMethod> EnabledMethods { get; set; } = new();
    public DateTime? SetupDate { get; set; }
    public DateTime? LastUsed { get; set; }
}