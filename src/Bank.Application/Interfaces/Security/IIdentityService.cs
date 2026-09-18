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
}

public class IdentityLoginResult : BaseResultDto
{
    public bool IsLockedOut { get; set; }
}

public class IdentityCreationResult : BaseResultDto
{
    public User? User { get; set; }
}
