namespace Bank.Application.Validators.Account;

/// <summary>
/// Validator for account number format
/// </summary>
public static class AccountNumberValidator
{
    /// <summary>
    /// Validates account number format
    /// </summary>
    /// <param name="accountNumber">Account number to validate</param>
    /// <returns>True if account number format is valid</returns>
    public static bool ValidateFormat(string accountNumber)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            return false;

        // Account number should be 8-17 digits
        var cleaned = accountNumber.Replace("-", "").Replace(" ", "");
        return cleaned.All(char.IsDigit) && cleaned.Length >= 8 && cleaned.Length <= 17;
    }
}
