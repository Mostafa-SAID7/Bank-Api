namespace Bank.Application.DTOs.Auth.TwoFactor;

/// <summary>
/// Result of a two-factor authentication token verification
/// </summary>
public class TwoFactorVerificationResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public bool RequiresAdditionalVerification { get; set; }
}

