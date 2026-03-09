using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Bank.Domain.Enums;

namespace Bank.Application.Utilities;

/// <summary>
/// Centralized helper for validation operations
/// </summary>
public static class ValidationHelper
{
    /// <summary>
    /// Validates IBAN checksum using mod-97 algorithm
    /// </summary>
    /// <param name="iban">IBAN to validate</param>
    /// <returns>True if IBAN is valid</returns>
    public static bool ValidateIbanChecksum(string iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
            return false;

        // Remove spaces and convert to uppercase
        iban = iban.Replace(" ", "").ToUpperInvariant();

        // IBAN must be between 15 and 34 characters
        if (iban.Length < 15 || iban.Length > 34)
            return false;

        // Move first 4 characters to the end
        var rearranged = iban[4..] + iban[..4];

        // Replace letters with numbers (A=10, B=11, ..., Z=35)
        var numericString = "";
        foreach (char c in rearranged)
        {
            if (char.IsLetter(c))
            {
                numericString += (c - 'A' + 10).ToString();
            }
            else if (char.IsDigit(c))
            {
                numericString += c;
            }
            else
            {
                return false;
            }
        }

        // Calculate mod 97
        return CalculateMod97(numericString) == 1;
    }

    /// <summary>
    /// Validates US routing number checksum
    /// </summary>
    /// <param name="routingNumber">9-digit routing number</param>
    /// <returns>True if routing number is valid</returns>
    public static bool ValidateUSRoutingNumberChecksum(string routingNumber)
    {
        if (string.IsNullOrWhiteSpace(routingNumber) || routingNumber.Length != 9)
            return false;

        if (!routingNumber.All(char.IsDigit))
            return false;

        var digits = routingNumber.Select(c => int.Parse(c.ToString())).ToArray();

        // ABA routing number checksum algorithm
        var checksum = (3 * (digits[0] + digits[3] + digits[6]) +
                       7 * (digits[1] + digits[4] + digits[7]) +
                       (digits[2] + digits[5] + digits[8])) % 10;

        return checksum == 0;
    }

    /// <summary>
    /// Performs basic sanctions check simulation
    /// </summary>
    /// <param name="name">Name to check</param>
    /// <param name="country">Country code</param>
    /// <returns>True if passed sanctions check</returns>
    public static bool PerformSanctionsCheck(string name, string? country = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        // Simulate sanctions list check
        var sanctionedNames = new[]
        {
            "BLOCKED PERSON",
            "SANCTIONED ENTITY",
            "PROHIBITED INDIVIDUAL"
        };

        var sanctionedCountries = new[]
        {
            "XX", // Placeholder for sanctioned countries
            "YY"
        };

        var normalizedName = name.ToUpperInvariant().Trim();
        
        // Check against sanctioned names
        if (sanctionedNames.Any(sanctioned => normalizedName.Contains(sanctioned)))
            return false;

        // Check against sanctioned countries
        if (!string.IsNullOrEmpty(country) && sanctionedCountries.Contains(country.ToUpperInvariant()))
            return false;

        return true;
    }

    /// <summary>
    /// Validates email format
    /// </summary>
    /// <param name="email">Email to validate</param>
    /// <returns>True if email format is valid</returns>
    public static bool ValidateEmailFormat(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            return emailRegex.IsMatch(email);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Validates phone number format
    /// </summary>
    /// <param name="phoneNumber">Phone number to validate</param>
    /// <returns>True if phone number format is valid</returns>
    public static bool ValidatePhoneNumberFormat(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        // Remove common formatting characters
        var cleaned = Regex.Replace(phoneNumber, @"[\s\-\(\)\+\.]", "");
        
        // Check if it's all digits and reasonable length
        return cleaned.All(char.IsDigit) && cleaned.Length >= 10 && cleaned.Length <= 15;
    }

    /// <summary>
    /// Validates account number format
    /// </summary>
    /// <param name="accountNumber">Account number to validate</param>
    /// <returns>True if account number format is valid</returns>
    public static bool ValidateAccountNumberFormat(string accountNumber)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            return false;

        // Account number should be 8-17 digits
        var cleaned = accountNumber.Replace("-", "").Replace(" ", "");
        return cleaned.All(char.IsDigit) && cleaned.Length >= 8 && cleaned.Length <= 17;
    }

    /// <summary>
    /// Validates credit card number using Luhn algorithm
    /// </summary>
    /// <param name="cardNumber">Credit card number</param>
    /// <returns>True if card number is valid</returns>
    public static bool ValidateCreditCardNumber(string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return false;

        // Remove spaces and dashes
        var cleaned = cardNumber.Replace(" ", "").Replace("-", "");
        
        if (!cleaned.All(char.IsDigit) || cleaned.Length < 13 || cleaned.Length > 19)
            return false;

        return ValidateLuhnChecksum(cleaned);
    }

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
    public static (bool IsValid, List<string> Errors) ValidatePasswordStrength(
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

    /// <summary>
    /// Validates PIN format
    /// </summary>
    /// <param name="pin">PIN to validate</param>
    /// <param name="expectedLength">Expected PIN length</param>
    /// <returns>True if PIN format is valid</returns>
    public static bool ValidatePinFormat(string pin, int expectedLength = 4)
    {
        if (string.IsNullOrWhiteSpace(pin))
            return false;

        return pin.Length == expectedLength && pin.All(char.IsDigit);
    }

    /// <summary>
    /// Validates amount is within acceptable range
    /// </summary>
    /// <param name="amount">Amount to validate</param>
    /// <param name="minAmount">Minimum allowed amount</param>
    /// <param name="maxAmount">Maximum allowed amount</param>
    /// <returns>True if amount is valid</returns>
    public static bool ValidateAmountRange(decimal amount, decimal minAmount = 0.01m, decimal? maxAmount = null)
    {
        if (amount < minAmount)
            return false;

        if (maxAmount.HasValue && amount > maxAmount.Value)
            return false;

        return true;
    }

    /// <summary>
    /// Validates currency code format (ISO 4217)
    /// </summary>
    /// <param name="currencyCode">Currency code to validate</param>
    /// <returns>True if currency code format is valid</returns>
    public static bool ValidateCurrencyCode(string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
            return false;

        // ISO 4217 currency codes are 3 uppercase letters
        return currencyCode.Length == 3 && currencyCode.All(char.IsLetter) && currencyCode == currencyCode.ToUpperInvariant();
    }

    #region Private Helper Methods

    /// <summary>
    /// Gets status message for bill payment status
    /// </summary>
    /// <param name="status">Bill payment status</param>
    /// <returns>Status message</returns>
    public static string GetBillPaymentStatusMessage(BillPaymentStatus status)
    {
        return status switch
        {
            BillPaymentStatus.Pending => "Payment is pending processing",
            BillPaymentStatus.Processing => "Payment is being processed",
            BillPaymentStatus.Processed => "Payment has been processed successfully",
            BillPaymentStatus.Delivered => "Payment has been delivered to the biller",
            BillPaymentStatus.Failed => "Payment processing failed",
            BillPaymentStatus.Cancelled => "Payment was cancelled",
            BillPaymentStatus.Returned => "Payment was returned by the biller",
            _ => "Unknown status"
        };
    }

    /// <summary>
    /// Generates a random boolean based on probability
    /// </summary>
    /// <param name="probability">Probability of returning true (0.0 to 1.0)</param>
    /// <returns>Random boolean</returns>
    public static bool GenerateRandomBoolean(double probability = 0.5)
    {
        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[4];
        rng.GetBytes(randomBytes);
        var randomValue = Math.Abs(BitConverter.ToInt32(randomBytes, 0)) / (double)int.MaxValue;
        return randomValue < probability;
    }

    /// <summary>
    /// Generates a random number within a specified range
    /// </summary>
    /// <param name="min">Minimum value (inclusive)</param>
    /// <param name="max">Maximum value (exclusive)</param>
    /// <returns>Random number</returns>
    public static int GenerateRandomNumber(int min, int max)
    {
        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[4];
        rng.GetBytes(randomBytes);
        var randomValue = Math.Abs(BitConverter.ToInt32(randomBytes, 0));
        return min + (randomValue % (max - min));
    }

    private static int CalculateMod97(string numericString)
    {
        var remainder = 0;
        foreach (char digit in numericString)
        {
            remainder = (remainder * 10 + (digit - '0')) % 97;
        }
        return remainder;
    }

    private static bool ValidateLuhnChecksum(string number)
    {
        var sum = 0;
        var alternate = false;

        for (int i = number.Length - 1; i >= 0; i--)
        {
            var digit = int.Parse(number[i].ToString());

            if (alternate)
            {
                digit *= 2;
                if (digit > 9)
                    digit = (digit % 10) + 1;
            }

            sum += digit;
            alternate = !alternate;
        }

        return sum % 10 == 0;
    }

    #endregion
}