namespace Bank.Application.Validators.Shared;

/// <summary>
/// Validator for IBAN (International Bank Account Number) format and checksum
/// </summary>
public static class IbanValidator
{
    /// <summary>
    /// Validates IBAN checksum using mod-97 algorithm
    /// </summary>
    /// <param name="iban">IBAN to validate</param>
    /// <returns>True if IBAN is valid</returns>
    public static bool ValidateChecksum(string iban)
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

    private static int CalculateMod97(string numericString)
    {
        var remainder = 0;
        foreach (char digit in numericString)
        {
            remainder = (remainder * 10 + (digit - '0')) % 97;
        }
        return remainder;
    }
}
