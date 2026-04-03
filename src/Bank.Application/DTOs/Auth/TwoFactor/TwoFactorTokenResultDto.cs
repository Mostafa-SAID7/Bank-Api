namespace Bank.Application.DTOs.Auth.TwoFactor;

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

