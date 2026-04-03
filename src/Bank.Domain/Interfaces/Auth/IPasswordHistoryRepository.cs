using Bank.Domain.Entities;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for PasswordHistory entity
/// </summary>
public interface IPasswordHistoryRepository : IRepository<PasswordHistory>
{
    /// <summary>
    /// Gets password history for a user
    /// </summary>
    Task<List<PasswordHistory>> GetPasswordHistoryAsync(Guid userId, int? limit = null);

    /// <summary>
    /// Gets recent passwords for a user (for history validation)
    /// </summary>
    Task<List<PasswordHistory>> GetRecentPasswordsAsync(Guid userId, int count);

    /// <summary>
    /// Gets the current password record for a user
    /// </summary>
    Task<PasswordHistory?> GetCurrentPasswordAsync(Guid userId);

    /// <summary>
    /// Marks current password as old for a user
    /// </summary>
    Task MarkCurrentPasswordAsOldAsync(Guid userId);

    /// <summary>
    /// Cleans up old password history entries, keeping only the specified count
    /// </summary>
    Task CleanupOldPasswordsAsync(Guid userId, int keepCount);

    /// <summary>
    /// Gets password history by hash (for duplicate checking)
    /// </summary>
    Task<PasswordHistory?> GetByPasswordHashAsync(Guid userId, string passwordHash);
}