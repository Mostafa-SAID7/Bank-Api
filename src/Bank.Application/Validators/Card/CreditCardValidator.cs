namespace Bank.Application.Validators.Card;

/// <summary>
/// Validator for credit card number format and checksum
/// </summary>
public static class CreditCardValidator
{
    /// <summary>
    /// Validates credit card number using Luhn algorithm
    /// </summary>
    /// <param name="cardNumber">Credit card number</param>
    /// <returns>True if card number is valid</returns>
    public static bool ValidateNumber(string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return false;

        // Remove spaces and dashes
        var cleaned = cardNumber.Replace(" ", "").Replace("-", "");
        
        if (!cleaned.All(char.IsDigit) || cleaned.Length < 13 || cleaned.Length > 19)
            return false;

        return ValidateLuhnChecksum(cleaned);
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
}
