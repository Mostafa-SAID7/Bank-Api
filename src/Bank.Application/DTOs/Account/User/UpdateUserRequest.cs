using System.ComponentModel.DataAnnotations;

namespace Bank.Application.DTOs.Account.User;

/// <summary>
/// Request DTO for updating user information
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// User ID to update
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Updated first name
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Updated last name
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Updated email address
    /// </summary>
    [EmailAddress]
    public string? Email { get; set; }

    /// <summary>
    /// New password (optional)
    /// </summary>
    [StringLength(100, MinimumLength = 8)]
    public string? NewPassword { get; set; }
}
