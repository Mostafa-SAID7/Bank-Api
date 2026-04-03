using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Password policy entity for defining password complexity and rotation requirements
/// </summary>
public class PasswordPolicy : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public PasswordComplexityLevel ComplexityLevel { get; private set; }
    public int MinimumLength { get; private set; }
    public int MaximumLength { get; private set; }
    public bool RequireUppercase { get; private set; }
    public bool RequireLowercase { get; private set; }
    public bool RequireDigits { get; private set; }
    public bool RequireSpecialCharacters { get; private set; }
    public string AllowedSpecialCharacters { get; private set; } = "!@#$%^&*()_+-=[]{}|;:,.<>?";
    public int MinimumUniqueCharacters { get; private set; }
    public bool PreventCommonPasswords { get; private set; }
    public bool PreventUserInfoInPassword { get; private set; }
    public int PasswordHistoryCount { get; private set; } // Number of previous passwords to remember
    public TimeSpan MaxPasswordAge { get; private set; } // How long before password must be changed
    public TimeSpan? MinPasswordAge { get; private set; } // Minimum time before password can be changed again
    public int MaxFailedAttempts { get; private set; }
    public TimeSpan LockoutDuration { get; private set; }
    public bool IsDefault { get; private set; }
    public bool IsActive { get; private set; }
    public string? Description { get; private set; }

    // Private constructor for EF Core
    private PasswordPolicy() { }

    public PasswordPolicy(
        string name,
        PasswordComplexityLevel complexityLevel,
        int minimumLength = 8,
        int maximumLength = 128,
        bool requireUppercase = true,
        bool requireLowercase = true,
        bool requireDigits = true,
        bool requireSpecialCharacters = false,
        int minimumUniqueCharacters = 4,
        bool preventCommonPasswords = true,
        bool preventUserInfoInPassword = true,
        int passwordHistoryCount = 5,
        TimeSpan? maxPasswordAge = null,
        TimeSpan? minPasswordAge = null,
        int maxFailedAttempts = 5,
        TimeSpan? lockoutDuration = null,
        bool isDefault = false,
        string? description = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        ComplexityLevel = complexityLevel;
        MinimumLength = minimumLength;
        MaximumLength = maximumLength;
        RequireUppercase = requireUppercase;
        RequireLowercase = requireLowercase;
        RequireDigits = requireDigits;
        RequireSpecialCharacters = requireSpecialCharacters;
        MinimumUniqueCharacters = minimumUniqueCharacters;
        PreventCommonPasswords = preventCommonPasswords;
        PreventUserInfoInPassword = preventUserInfoInPassword;
        PasswordHistoryCount = passwordHistoryCount;
        MaxPasswordAge = maxPasswordAge ?? TimeSpan.FromHours(23); // Default 23 hours (changed from 90 days to fit in time data type)
        MinPasswordAge = minPasswordAge ?? TimeSpan.FromHours(1); // Default 1 hour (changed from 24 hours to fit in time data type)
        MaxFailedAttempts = maxFailedAttempts;
        LockoutDuration = lockoutDuration ?? TimeSpan.FromMinutes(30); // Default 30 minutes
        IsDefault = isDefault;
        IsActive = true;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }

    public static PasswordPolicy CreateBasicPolicy()
    {
        return new PasswordPolicy(
            "Basic",
            PasswordComplexityLevel.Basic,
            minimumLength: 6,
            requireUppercase: false,
            requireLowercase: true,
            requireDigits: true,
            requireSpecialCharacters: false,
            minimumUniqueCharacters: 3,
            passwordHistoryCount: 3,
            maxPasswordAge: TimeSpan.FromHours(23), // Changed from 180 days to 23 hours to fit in time data type
            maxFailedAttempts: 10,
            lockoutDuration: TimeSpan.FromMinutes(15),
            description: "Basic password policy for standard users"
        );
    }

    public static PasswordPolicy CreateStandardPolicy()
    {
        return new PasswordPolicy(
            "Standard",
            PasswordComplexityLevel.Standard,
            minimumLength: 8,
            requireUppercase: true,
            requireLowercase: true,
            requireDigits: true,
            requireSpecialCharacters: false,
            minimumUniqueCharacters: 4,
            passwordHistoryCount: 5,
            maxPasswordAge: TimeSpan.FromHours(22), // Changed from 90 days to 22 hours to fit in time data type
            maxFailedAttempts: 5,
            lockoutDuration: TimeSpan.FromMinutes(30),
            isDefault: true,
            description: "Standard password policy for most users"
        );
    }

    public static PasswordPolicy CreateStrongPolicy()
    {
        return new PasswordPolicy(
            "Strong",
            PasswordComplexityLevel.Strong,
            minimumLength: 10,
            requireUppercase: true,
            requireLowercase: true,
            requireDigits: true,
            requireSpecialCharacters: true,
            minimumUniqueCharacters: 6,
            passwordHistoryCount: 8,
            maxPasswordAge: TimeSpan.FromHours(20), // Changed from 60 days to 20 hours to fit in time data type
            maxFailedAttempts: 3,
            lockoutDuration: TimeSpan.FromHours(1),
            description: "Strong password policy for privileged users"
        );
    }

    public static PasswordPolicy CreateEnterprisePolicy()
    {
        return new PasswordPolicy(
            "Enterprise",
            PasswordComplexityLevel.Enterprise,
            minimumLength: 12,
            maximumLength: 64,
            requireUppercase: true,
            requireLowercase: true,
            requireDigits: true,
            requireSpecialCharacters: true,
            minimumUniqueCharacters: 8,
            passwordHistoryCount: 12,
            maxPasswordAge: TimeSpan.FromHours(18), // Changed from 30 days to 18 hours to fit in time data type
            minPasswordAge: TimeSpan.FromHours(1), // Changed from 1 day to 1 hour to fit in time data type
            maxFailedAttempts: 3,
            lockoutDuration: TimeSpan.FromHours(2),
            description: "Enterprise password policy for administrators and high-privilege users"
        );
    }

    public void UpdatePolicy(
        int? minimumLength = null,
        int? maximumLength = null,
        bool? requireUppercase = null,
        bool? requireLowercase = null,
        bool? requireDigits = null,
        bool? requireSpecialCharacters = null,
        int? minimumUniqueCharacters = null,
        bool? preventCommonPasswords = null,
        bool? preventUserInfoInPassword = null,
        int? passwordHistoryCount = null,
        TimeSpan? maxPasswordAge = null,
        TimeSpan? minPasswordAge = null,
        int? maxFailedAttempts = null,
        TimeSpan? lockoutDuration = null)
    {
        if (minimumLength.HasValue) MinimumLength = minimumLength.Value;
        if (maximumLength.HasValue) MaximumLength = maximumLength.Value;
        if (requireUppercase.HasValue) RequireUppercase = requireUppercase.Value;
        if (requireLowercase.HasValue) RequireLowercase = requireLowercase.Value;
        if (requireDigits.HasValue) RequireDigits = requireDigits.Value;
        if (requireSpecialCharacters.HasValue) RequireSpecialCharacters = requireSpecialCharacters.Value;
        if (minimumUniqueCharacters.HasValue) MinimumUniqueCharacters = minimumUniqueCharacters.Value;
        if (preventCommonPasswords.HasValue) PreventCommonPasswords = preventCommonPasswords.Value;
        if (preventUserInfoInPassword.HasValue) PreventUserInfoInPassword = preventUserInfoInPassword.Value;
        if (passwordHistoryCount.HasValue) PasswordHistoryCount = passwordHistoryCount.Value;
        if (maxPasswordAge.HasValue) MaxPasswordAge = maxPasswordAge.Value;
        if (minPasswordAge.HasValue) MinPasswordAge = minPasswordAge.Value;
        if (maxFailedAttempts.HasValue) MaxFailedAttempts = maxFailedAttempts.Value;
        if (lockoutDuration.HasValue) LockoutDuration = lockoutDuration.Value;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
        IsDefault = false; // Can't be default if inactive
    }

    public void SetAsDefault()
    {
        IsDefault = true;
        IsActive = true; // Must be active to be default
    }
}