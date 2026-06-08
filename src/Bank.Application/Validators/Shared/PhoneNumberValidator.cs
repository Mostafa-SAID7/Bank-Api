using Bank.Application.Helpers.Shared;

namespace Bank.Application.Validators.Shared;

/// <summary>
/// Validator for phone number format (delegates to centralized ValidationHelper)
/// </summary>
public static class PhoneNumberValidator
{
    /// <summary>
    /// Validates phone number format
    /// </summary>
    /// <param name="phoneNumber">Phone number to validate</param>
    /// <returns>True if phone number format is valid</returns>
    public static bool ValidateFormat(string phoneNumber)
    {
        return ValidationHelper.IsValidPhoneNumber(phoneNumber);
    }
}
