using System.Text.RegularExpressions;

namespace Bank.Application.Helpers.Account;

/// <summary>
/// Centralized helper for validating international banking codes (SWIFT, IBAN, Routing Numbers)
/// Eliminates duplication of regex patterns and validation logic across services
/// </summary>
public static class BankingCodeValidationHelper
{
    // Regex timeout to prevent ReDoS attacks
    private static readonly TimeSpan RegexTimeout = TimeSpan.FromMilliseconds(500);

    // SWIFT code pattern: 4 letters (bank) + 2 letters (country) + 2 alphanumeric (location) + optional 3 alphanumeric (branch)
    private static readonly Regex SwiftCodePattern = new(@"^[A-Z]{4}[A-Z]{2}[A-Z0-9]{2}([A-Z0-9]{3})?$", RegexOptions.Compiled, RegexTimeout);

    // Default branch code when not specified in SWIFT code
    private const string DefaultBranchCode = "XXX";

    // IBAN patterns by country (simplified - in production, use comprehensive IBAN registry)
    private static readonly Dictionary<string, (int Length, Regex Pattern)> IbanPatterns = new()
    {
        { "AD", (24, new Regex(@"^AD\d{2}\d{4}\d{4}\d{12}$", RegexOptions.None, RegexTimeout)) },
        { "AE", (23, new Regex(@"^AE\d{2}\d{3}\d{16}$", RegexOptions.None, RegexTimeout)) },
        { "AL", (28, new Regex(@"^AL\d{2}\d{8}[A-Z0-9]{16}$", RegexOptions.None, RegexTimeout)) },
        { "AT", (20, new Regex(@"^AT\d{2}\d{5}\d{11}$", RegexOptions.None, RegexTimeout)) },
        { "BE", (16, new Regex(@"^BE\d{2}\d{3}\d{7}\d{2}$", RegexOptions.None, RegexTimeout)) },
        { "BG", (22, new Regex(@"^BG\d{2}[A-Z]{4}\d{6}[A-Z0-9]{8}$", RegexOptions.None, RegexTimeout)) },
        { "CH", (21, new Regex(@"^CH\d{2}\d{5}[A-Z0-9]{12}$", RegexOptions.None, RegexTimeout)) },
        { "DE", (22, new Regex(@"^DE\d{2}\d{8}\d{10}$", RegexOptions.None, RegexTimeout)) },
        { "ES", (24, new Regex(@"^ES\d{2}\d{4}\d{4}\d{1}\d{1}\d{10}$", RegexOptions.None, RegexTimeout)) },
        { "FR", (27, new Regex(@"^FR\d{2}\d{5}\d{5}[A-Z0-9]{11}\d{2}$", RegexOptions.None, RegexTimeout)) },
        { "GB", (22, new Regex(@"^GB\d{2}[A-Z]{4}\d{6}\d{8}$", RegexOptions.None, RegexTimeout)) },
        { "IT", (27, new Regex(@"^IT\d{2}[A-Z]{1}\d{5}\d{5}[A-Z0-9]{12}$", RegexOptions.None, RegexTimeout)) },
        { "NL", (18, new Regex(@"^NL\d{2}[A-Z]{4}\d{10}$", RegexOptions.None, RegexTimeout)) },
        { "US", (0, new Regex(@"^$", RegexOptions.None, RegexTimeout)) } // US doesn't use IBAN
    };

    /// <summary>
    /// Validates SWIFT code format and structure
    /// </summary>
    public static (bool IsValid, string BankCode, string CountryCode, string LocationCode, string BranchCode) ValidateSwiftCodeFormat(string? swiftCode)
    {
        if (string.IsNullOrEmpty(swiftCode))
            return (false, "", "", "", "");

        swiftCode = swiftCode.ToUpper().Replace(" ", "");

        if (!SwiftCodePattern.IsMatch(swiftCode))
            return (false, "", "", "", "");

        var bankCode = swiftCode[..4];
        var countryCode = swiftCode.Substring(4, 2);
        var locationCode = swiftCode.Substring(6, 2);
        var branchCode = swiftCode.Length > 8 ? swiftCode.Substring(8, 3) : DefaultBranchCode;

        return (true, bankCode, countryCode, locationCode, branchCode);
    }

    /// <summary>
    /// Validates IBAN format and checksum for country-specific rules
    /// </summary>
    public static (bool IsValid, string CountryCode, bool ChecksumValid, List<string> Errors) ValidateIbanFormat(string? iban)
    {
        var errors = new List<string>();

        if (string.IsNullOrEmpty(iban))
        {
            errors.Add("IBAN is required");
            return (false, "", false, errors);
        }

        iban = iban.ToUpper().Replace(" ", "");

        if (iban.Length < 15 || iban.Length > 34)
        {
            errors.Add("IBAN length must be between 15 and 34 characters");
            return (false, "", false, errors);
        }

        var countryCode = iban[..2];

        if (!IbanPatterns.TryGetValue(countryCode, out var pattern))
        {
            errors.Add($"Unsupported country code: {countryCode}");
            return (false, countryCode, false, errors);
        }

        if (pattern.Length > 0 && iban.Length != pattern.Length)
        {
            errors.Add($"Invalid IBAN length for {countryCode}. Expected {pattern.Length}, got {iban.Length}");
            return (false, countryCode, false, errors);
        }

        if (!pattern.Pattern.IsMatch(iban))
        {
            errors.Add($"Invalid IBAN format for {countryCode}");
            return (false, countryCode, false, errors);
        }

        var checksumValid = ValidateIbanChecksum(iban);
        if (!checksumValid)
        {
            errors.Add("Invalid IBAN checksum");
            return (false, countryCode, false, errors);
        }

        return (true, countryCode, checksumValid, errors);
    }

    /// <summary>
    /// Validates US routing number format and checksum
    /// </summary>
    public static (bool IsValid, List<string> Errors) ValidateRoutingNumberFormat(string? routingNumber)
    {
        var errors = new List<string>();

        if (string.IsNullOrEmpty(routingNumber))
        {
            errors.Add("Routing number is required");
            return (false, errors);
        }

        routingNumber = routingNumber.Replace("-", "").Replace(" ", "");

        if (routingNumber.Length != 9 || !routingNumber.All(char.IsDigit))
        {
            errors.Add("US routing number must be 9 digits");
            return (false, errors);
        }

        if (!ValidateUSRoutingNumberChecksum(routingNumber))
        {
            errors.Add("Invalid routing number checksum");
            return (false, errors);
        }

        return (true, errors);
    }

    /// <summary>
    /// Extracts IBAN components (bank code, account number) by country
    /// </summary>
    public static (string BankCode, string AccountNumber) ExtractIbanComponents(string iban, string countryCode)
    {
        return countryCode.ToUpper() switch
        {
            "DE" => (iban.Substring(4, 8), iban.Substring(12, 10)), // Germany
            "GB" => (iban.Substring(4, 4), iban.Substring(14, 8)), // UK
            "FR" => (iban.Substring(4, 5), iban.Substring(14, 11)), // France
            _ => (iban.Length > 8 ? iban.Substring(4, 4) : "", iban.Length > 12 ? iban[12..] : "")
        };
    }

    /// <summary>
    /// Validates IBAN checksum using mod-97 algorithm
    /// </summary>
    private static bool ValidateIbanChecksum(string iban)
    {
        // Move first 4 characters to end
        var rearranged = iban[4..] + iban[..4];

        // Replace letters with numbers (A=10, B=11, ..., Z=35)
        var numericString = "";
        foreach (char c in rearranged)
        {
            if (char.IsLetter(c))
                numericString += (c - 'A' + 10).ToString();
            else
                numericString += c;
        }

        return CalculateMod97(numericString) == 1;
    }

    /// <summary>
    /// Calculates mod 97 for IBAN checksum validation
    /// </summary>
    private static int CalculateMod97(string numericString)
    {
        var remainder = 0;
        foreach (char digit in numericString)
        {
            remainder = (remainder * 10 + (digit - '0')) % 97;
        }
        return remainder;
    }

    /// <summary>
    /// Validates US routing number checksum using weighted algorithm
    /// </summary>
    private static bool ValidateUSRoutingNumberChecksum(string routingNumber)
    {
        var weights = new[] { 3, 7, 1, 3, 7, 1, 3, 7, 1 };
        var sum = 0;

        for (int i = 0; i < 9; i++)
        {
            sum += (routingNumber[i] - '0') * weights[i];
        }

        return sum % 10 == 0;
    }
}
