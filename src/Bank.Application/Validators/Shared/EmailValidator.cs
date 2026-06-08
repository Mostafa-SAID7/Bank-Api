using Bank.Application.Helpers.Shared;

namespace Bank.Application.Validators.Shared;

/// <summary>
/// Validator for email format (delegates to centralized ValidationHelper)
/// </summary>
public static class EmailValidator
{
    /// <summary>
    /// Validates email format
    /// </summary>
    /// <param name="email">Email to validate</param>
    /// <returns>True if email format is valid</returns>
    public static bool ValidateFormat(string email)
    {
        return ValidationHelper.IsValidEmail(email);
    }
}
