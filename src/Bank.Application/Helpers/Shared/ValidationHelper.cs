using System.Text.RegularExpressions;

namespace Bank.Application.Helpers.Shared;

/// <summary>
/// Centralized validation helper for email and phone number validation.
/// Provides consistent validation across all services (EmailService, SmsService, etc.)
/// </summary>
public static class ValidationHelper
{
    private static readonly TimeSpan RegexTimeout = TimeSpan.FromMilliseconds(500);

    /// <summary>
    /// Validates if the provided string is a valid email address.
    /// Uses regex pattern matching with timeout protection.
    /// </summary>
    /// <param name="email">The email address to validate</param>
    /// <returns>True if email is valid format, false otherwise</returns>
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Use regex for basic email validation: localpart@domain.extension
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, RegexTimeout);
            return emailRegex.IsMatch(email);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Validates if the provided string is a valid phone number.
    /// Supports international format with country code prefix.
    /// Format: +1-9 to +999999999999999 (international E.164 format)
    /// </summary>
    /// <param name="phoneNumber">The phone number to validate</param>
    /// <returns>True if phone number is valid format, false otherwise</returns>
    public static bool IsValidPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        try
        {
            // Basic phone number validation - supports international format (E.164)
            // Format: +1 to +999999999999999 with optional separators
            var phoneRegex = new Regex(@"^\+?[1-9]\d{1,14}$", RegexOptions.None, RegexTimeout);
            var cleanedNumber = phoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
            return phoneRegex.IsMatch(cleanedNumber);
        }
        catch
        {
            return false;
        }
    }
}
