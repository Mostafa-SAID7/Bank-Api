using System.ComponentModel.DataAnnotations;

namespace Bank.Application.DTOs.Auth.TwoFactor;

/// <summary>
/// Request DTO for creating a 2FA token
/// </summary>
public class CreateTwoFactorTokenRequest
{
    /// <summary>
    /// User ID to create token for
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Purpose of the 2FA token (e.g., "Login", "Transaction")
    /// </summary>
    [Required]
    public string Purpose { get; set; } = string.Empty;
}
