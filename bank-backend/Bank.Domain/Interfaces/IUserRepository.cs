using Bank.Domain.Entities;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for User entity (extends Identity User)
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets a user by ID
    /// </summary>
    Task<User?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets a user by username
    /// </summary>
    Task<User?> GetByUsernameAsync(string username);

    /// <summary>
    /// Gets a user by email
    /// </summary>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Gets all users
    /// </summary>
    Task<List<User>> GetAllUsersAsync();

    /// <summary>
    /// Updates a user
    /// </summary>
    Task UpdateAsync(User user);

    /// <summary>
    /// Adds a user
    /// </summary>
    Task AddAsync(User user);

    /// <summary>
    /// Gets users by role
    /// </summary>
    Task<List<User>> GetUsersByRoleAsync(string roleName);
}