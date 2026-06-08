namespace Bank.Application.DTOs.Auth.TwoFactor;

/// <summary>
/// Data transfer object for 2FA token information
/// </summary>
public class TwoFactorTokenDto
{
    /// <summary>
    /// Token ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User ID associated with the token
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The 2FA token value
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// When the token expires
    /// </summary>
    public DateTime ExpiresAt { get; set; }
}
