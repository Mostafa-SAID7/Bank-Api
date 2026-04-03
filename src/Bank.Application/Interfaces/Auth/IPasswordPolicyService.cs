using Bank.Application.DTOs;
using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for managing password policies and validation
/// </summary>
public interface IPasswordPolicyService
{
    /// <summary>
    /// Validates a password against the specified policy
    /// </summary>
    Task<PasswordValidationResult> ValidatePasswordAsync(string password, Guid userId, PasswordComplexityLevel? complexityLevel = null);

    /// <summary>
    /// Validates a password against a specific policy
    /// </summary>
    Task<PasswordValidationResult> ValidatePasswordAsync(string password, Guid userId, Guid policyId);

    /// <summary>
    /// Gets the default password policy
    /// </summary>
    Task<PasswordPolicy?> GetDefaultPasswordPolicyAsync();

    /// <summary>
    /// Gets a password policy by complexity level
    /// </summary>
    Task<PasswordPolicy?> GetPasswordPolicyAsync(PasswordComplexityLevel complexityLevel);

    /// <summary>
    /// Gets all active password policies
    /// </summary>
    Task<List<PasswordPolicy>> GetActivePasswordPoliciesAsync();

    /// <summary>
    /// Creates a new password policy
    /// </summary>
    Task<PasswordPolicy> CreatePasswordPolicyAsync(PasswordPolicy policy);

    /// <summary>
    /// Updates an existing password policy
    /// </summary>
    Task<bool> UpdatePasswordPolicyAsync(Guid policyId, PasswordPolicy updatedPolicy);

    /// <summary>
    /// Sets a policy as the default
    /// </summary>
    Task<bool> SetDefaultPasswordPolicyAsync(Guid policyId);

    /// <summary>
    /// Checks if a user's password needs to be changed based on age policy
    /// </summary>
    Task<bool> IsPasswordChangeRequiredAsync(Guid userId);

    /// <summary>
    /// Records a password change in history
    /// </summary>
    Task RecordPasswordChangeAsync(Guid userId, string passwordHash, string? passwordSalt = null);

    /// <summary>
    /// Checks if a password has been used recently (password history check)
    /// </summary>
    Task<bool> IsPasswordRecentlyUsedAsync(Guid userId, string passwordHash);

    /// <summary>
    /// Gets password history for a user
    /// </summary>
    Task<List<PasswordHistory>> GetPasswordHistoryAsync(Guid userId, int? limit = null);

    /// <summary>
    /// Cleans up old password history entries based on policy
    /// </summary>
    Task CleanupPasswordHistoryAsync(Guid userId, int keepCount);

    /// <summary>
    /// Generates a secure password that meets policy requirements
    /// </summary>
    Task<string> GenerateSecurePasswordAsync(PasswordComplexityLevel complexityLevel);

    /// <summary>
    /// Checks if a password is in the common passwords list
    /// </summary>
    bool IsCommonPassword(string password);

    /// <summary>
    /// Checks if a password contains user information
    /// </summary>
    bool ContainsUserInfo(string password, User user);
}