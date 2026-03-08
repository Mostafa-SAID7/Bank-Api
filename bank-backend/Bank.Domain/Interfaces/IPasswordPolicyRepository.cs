using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for PasswordPolicy entity
/// </summary>
public interface IPasswordPolicyRepository : IRepository<PasswordPolicy>
{
    /// <summary>
    /// Gets the default password policy
    /// </summary>
    Task<PasswordPolicy?> GetDefaultPolicyAsync();

    /// <summary>
    /// Gets password policy by complexity level
    /// </summary>
    Task<PasswordPolicy?> GetByComplexityLevelAsync(PasswordComplexityLevel complexityLevel);

    /// <summary>
    /// Gets all active password policies
    /// </summary>
    Task<List<PasswordPolicy>> GetActivePoliciesAsync();

    /// <summary>
    /// Clears the default flag from all policies
    /// </summary>
    Task ClearDefaultPolicyAsync();

    /// <summary>
    /// Gets policy by name
    /// </summary>
    Task<PasswordPolicy?> GetByNameAsync(string name);
}