using Bank.Application.DTOs.Common;
using Bank.Domain.Entities;

namespace Bank.Application.Interfaces.Security;

/// <summary>
/// Abstracted identity service to prevent Application layer from depending on ASP.NET Core Identity.
/// </summary>
public interface IIdentityService
{
    Task<User?> FindByIdAsync(Guid userId);
    Task<User?> FindByEmailAsync(string email);
    Task<IList<string>> GetRolesAsync(User user);
    
    /// <summary>
    /// Validates user credentials. Checks password and handles lockout.
    /// </summary>
    Task<IdentityLoginResult> ValidateCredentialsAsync(User user, string password);
    
    /// <summary>
    /// Creates a new user with the given details and default role.
    /// </summary>
    Task<IdentityCreationResult> CreateUserAsync(string username, string email, string password, string role = "User");

    /// <summary>
    /// Hashes a password for a user using the canonical password hasher.
    /// </summary>
    string HashPassword(User user, string password);

    /// <summary>
    /// Verifies a provided password against a hashed password using the canonical password hasher.
    /// </summary>
    bool VerifyPassword(User user, string hashedPassword, string providedPassword);
}

public class IdentityLoginResult : BaseResultDto
{
    public bool IsLockedOut { get; set; }
}

public class IdentityCreationResult : BaseResultDto
{
    public User? User { get; set; }
}
