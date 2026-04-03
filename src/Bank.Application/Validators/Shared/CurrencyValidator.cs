namespace Bank.Application.Validators.Shared;

/// <summary>
/// Validator for currency code format (ISO 4217)
/// </summary>
public static class CurrencyValidator
{
    /// <summary>
    /// Validates currency code format (ISO 4217)
    /// </summary>
    /// <param name="currencyCode">Currency code to validate</param>
    /// <returns>True if currency code format is valid</returns>
    public static bool ValidateCode(string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
            return false;

        // ISO 4217 currency codes are 3 uppercase letters
        return currencyCode.Length == 3 && currencyCode.All(char.IsLetter) && currencyCode == currencyCode.ToUpperInvariant();
    }
}
