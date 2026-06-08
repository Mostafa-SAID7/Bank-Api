namespace Bank.Application.Helpers.Shared;

/// <summary>
/// Centralized masking helper for safely logging sensitive data.
/// Provides consistent masking across all services (SmsService, Controllers, etc.)
/// SECURITY: Never log full phone numbers, card numbers, or account numbers
/// </summary>
public static class MaskingHelper
{
    /// <summary>
    /// Masks a phone number for safe logging: keeps country code prefix and last 4 digits.
    /// e.g. "+12025551234" → "+1*****1234"
    /// Returns "[empty]" if input is null/empty, "[redacted]" if number is too short.
    /// </summary>
    /// <param name="phoneNumber">The phone number to mask</param>
    /// <returns>Masked phone number safe for logging</returns>
    public static string MaskPhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber)) 
            return "[empty]";

        var cleaned = phoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
        if (cleaned.Length <= 4) 
            return "[redacted]";

        var prefix = cleaned.StartsWith('+') ? cleaned[..2] : cleaned[..1];
        var suffix = cleaned[^4..];
        var masked = new string('*', Math.Max(0, cleaned.Length - prefix.Length - 4));
        return $"{prefix}{masked}{suffix}";
    }

    /// <summary>
    /// Masks a card number for safe logging: keeps first 4 digits and last 4 digits.
    /// e.g. "4532-1234-5678-9010" → "4532-****-****-9010"
    /// </summary>
    /// <param name="cardNumber">The card number to mask</param>
    /// <returns>Masked card number safe for logging (first 4 and last 4 digits visible)</returns>
    public static string MaskCardNumber(string? cardNumber)
    {
        if (string.IsNullOrEmpty(cardNumber))
            return "[empty]";

        var cleaned = cardNumber.Replace(" ", "").Replace("-", "");
        if (cleaned.Length < 8)
            return "[redacted]";

        var first4 = cleaned[..4];
        var last4 = cleaned[^4..];
        var masked = new string('*', cleaned.Length - 8);
        
        // Reconstruct with original format if it had dashes
        if (cardNumber.Contains('-'))
        {
            return $"{first4}-{string.Join("-", masked.Chunk(4).Select(c => new string(c)))}-{last4}";
        }

        return $"{first4}{masked}{last4}";
    }

    /// <summary>
    /// Masks an account number for safe logging: keeps first 2 characters and last 3 digits.
    /// e.g. "ACC123456789012" → "AC*****789012"
    /// </summary>
    /// <param name="accountNumber">The account number to mask</param>
    /// <returns>Masked account number safe for logging</returns>
    public static string MaskAccountNumber(string? accountNumber)
    {
        if (string.IsNullOrEmpty(accountNumber))
            return "[empty]";

        if (accountNumber.Length <= 5)
            return "[redacted]";

        var prefix = accountNumber[..2];
        var suffix = accountNumber[^3..];
        var masked = new string('*', accountNumber.Length - 5);
        return $"{prefix}{masked}{suffix}";
    }

    /// <summary>
    /// Masks an email address for safe logging: shows first character and domain only.
    /// e.g. "user@example.com" → "u***@example.com"
    /// </summary>
    /// <param name="email">The email to mask</param>
    /// <returns>Masked email address safe for logging</returns>
    public static string MaskEmail(string? email)
    {
        if (string.IsNullOrEmpty(email))
            return "[empty]";

        var parts = email.Split('@');
        if (parts.Length != 2)
            return "[invalid]";

        var localPart = parts[0];
        var domain = parts[1];

        if (localPart.Length <= 1)
            return $"{localPart}@{domain}";

        var first = localPart[0];
        var masked = new string('*', localPart.Length - 1);
        return $"{first}{masked}@{domain}";
    }

    /// <summary>
    /// Masks a generic sensitive value: shows only first and last 2 characters.
    /// e.g. "secret123456" → "se**********56"
    /// Used for OTP codes, tokens, and other sensitive strings.
    /// </summary>
    /// <param name="value">The sensitive value to mask</param>
    /// <returns>Masked value safe for logging</returns>
    public static string MaskSensitiveValue(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return "[empty]";

        if (value.Length <= 4)
            return "[redacted]";

        var first2 = value[..2];
        var last2 = value[^2..];
        var masked = new string('*', value.Length - 4);
        return $"{first2}{masked}{last2}";
    }
}
