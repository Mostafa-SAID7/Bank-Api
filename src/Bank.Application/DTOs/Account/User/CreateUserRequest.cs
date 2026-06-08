using System.ComponentModel.DataAnnotations;

namespace Bank.Application.DTOs.Account.User;

/// <summary>
/// Request DTO for creating a new user account
/// </summary>
public class CreateUserRequest
{
    /// <summary>
    /// Username for the account
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Email address
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Password
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// First name
    /// </summary>
    [Required]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Last name
    /// </summary>
    [Required]
    public string LastName { get; set; } = string.Empty;
}
