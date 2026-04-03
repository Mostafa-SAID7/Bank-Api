namespace Bank.Application.Validators.Auth;

/// <summary>
/// Validator for password strength requirements
/// </summary>
public static class PasswordValidator
{
    /// <summary>
    /// Validates password strength
    /// </summary>
    /// <param name="password">Password to validate</param>
    /// <param name="minLength">Minimum length requirement</param>
    /// <param name="requireUppercase">Require uppercase letters</param>
    /// <param name="requireLowercase">Require lowercase letters</param>
    /// <param name="requireNumbers">Require numbers</param>
    /// <param name="requireSpecialChars">Require special characters</param>
    /// <returns>Validation result with success status and error messages</returns>
    public static (bool IsValid, List<string> Errors) ValidateStrength(
        string password, int minLength = 8, bool requireUppercase = true,
        bool requireLowercase = true, bool requireNumbers = true, bool requireSpecialChars = true)
    {
        var errors = new List<string>();

        if (string.IsNullOrEmpty(password))
        {
            errors.Add("Password is required");
            return (false, errors);
        }

        if (password.Length < minLength)
            errors.Add($"Password must be at least {minLength} characters long");

        if (requireUppercase && !password.Any(char.IsUpper))
            errors.Add("Password must contain at least one uppercase letter");

        if (requireLowercase && !password.Any(char.IsLower))
            errors.Add("Password must contain at least one lowercase letter");

        if (requireNumbers && !password.Any(char.IsDigit))
            errors.Add("Password must contain at least one number");

        if (requireSpecialChars && !password.Any(c => !char.IsLetterOrDigit(c)))
            errors.Add("Password must contain at least one special character");

        return (errors.Count == 0, errors);
    }
}
