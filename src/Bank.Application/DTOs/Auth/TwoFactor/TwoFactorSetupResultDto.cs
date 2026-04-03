namespace Bank.Application.DTOs.Auth.TwoFactor;

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

