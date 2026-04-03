namespace Bank.Application.Validators.Card;

/// <summary>
/// Validator for PIN (Personal Identification Number) format
/// </summary>
public static class PinValidator
{
    /// <summary>
    /// Validates PIN format
    /// </summary>
    /// <param name="pin">PIN to validate</param>
    /// <param name="expectedLength">Expected PIN length</param>
    /// <returns>True if PIN format is valid</returns>
    public static bool ValidateFormat(string pin, int expectedLength = 4)
    {
        if (string.IsNullOrWhiteSpace(pin))
            return false;

        return pin.Length == expectedLength && pin.All(char.IsDigit);
    }
}
