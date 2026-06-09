using Bank.Domain.Enums;

namespace Bank.Application.Helpers.Shared;

/// <summary>
/// Consolidated helper for generating secure tokens, codes, and identifiers.
/// Combines functionality from TokenGenerationHelper and RandomCodeGenerator.
/// </summary>
public static class TokenHelper
{
    #region Token Generation

    /// <summary>
    /// Generates a secure random token
    /// </summary>
    public static string GenerateSecureToken(int length = 32)
    {
        return Auth.AuthGeneratorHelper.GenerateSecureToken(length);
    }

    /// <summary>
    /// Generates a numeric token code
    /// </summary>
    public static string GenerateNumericToken(int length = 6)
    {
        return Auth.AuthGeneratorHelper.GenerateNumericToken(length);
    }

    /// <summary>
    /// Generates an external reference number
    /// </summary>
    public static string GenerateExternalReference()
    {
        return $"EXT-{Guid.NewGuid().ToString("N")[..12].ToUpper()}";
    }

    /// <summary>
    /// Generates a confirmation number
    /// </summary>
    public static string GenerateConfirmationNumber()
    {
        return $"CNF-{Guid.NewGuid().ToString("N")[..10].ToUpper()}";
    }

    /// <summary>
    /// Generates a random card PIN (usually 4 or 6 digits)
    /// </summary>
    public static string GenerateRandomPin(int length = 4)
    {
        return GenerateNumericToken(length);
    }

    #endregion

    #region Identifier Generation

    /// <summary>
    /// Generates a unique deposit number (FD + YYYYMMDD + 4-digit random)
    /// </summary>
    public static string GenerateDepositNumber()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd");
        var random = GenerateRandomNumber(4);
        return $"FD{timestamp}{random}";
    }

    /// <summary>
    /// Generates a unique deposit certificate number (DC + YYYYMMDD + 5-digit random)
    /// </summary>
    public static string GenerateCertificateNumber()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd");
        var random = GenerateRandomNumber(5);
        return $"DC{timestamp}{random}";
    }

    /// <summary>
    /// Generates a unique maturity notice number based on type
    /// Format: MN[TypeCode]YYYYMMDD[4-digit random]
    /// </summary>
    public static string GenerateNoticeNumber(MaturityNoticeType noticeType)
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd");
        var random = GenerateRandomNumber(4);
        var typeCode = GetNoticeTypeCode(noticeType);
        return $"MN{typeCode}{timestamp}{random}";
    }

    /// <summary>
    /// Generates a unique deposit transaction reference (DT + YYYYMMDDHHmmss + 3-digit random)
    /// </summary>
    public static string GenerateTransactionReference()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = GenerateRandomNumber(3);
        return $"DT{timestamp}{random}";
    }

    /// <summary>
    /// Generates a unique payment receipt number (RCP-YYYYMMDDHHmmss-4-digit random)
    /// </summary>
    public static string GenerateReceiptNumber()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = GenerateRandomNumber(4);
        return $"RCP-{timestamp}-{random}";
    }

    /// <summary>
    /// Generates a unique loan number (LN[YYYY][6-digit sequential])
    /// Note: Call GenerateNextLoanNumberAsync for async version with database query
    /// </summary>
    public static string GenerateLoanNumber(int nextSequence)
    {
        var year = DateTime.UtcNow.Year;
        return $"LN{year}{nextSequence:D6}";
    }

    /// <summary>
    /// Generates a masked card number for display (****-****-****-LastFour)
    /// </summary>
    public static string GenerateMaskedCardNumber(string cardNumber)
    {
        if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 4)
            return "****-****-****-****";

        var lastFour = cardNumber.Substring(cardNumber.Length - 4);
        return $"****-****-****-{lastFour}";
    }

    /// <summary>
    /// Generates a confirmation number with timestamp and random component
    /// </summary>
    /// <param name="prefix">Optional prefix for the confirmation number</param>
    /// <returns>Confirmation number</returns>
    public static string GeneratePaymentConfirmationNumber(string prefix = "CNF")
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var randomNumber = GenerateRandomNumber(6);
        return $"{prefix}{timestamp}{randomNumber}";
    }

    /// <summary>
    /// Generates an external reference for payment processing
    /// </summary>
    /// <returns>External reference string</returns>
    public static string GeneratePaymentExternalReference()
    {
        return $"EXT-{Guid.NewGuid():N}"[..16];
    }

    #endregion

    #region Random Code Generation

    /// <summary>
    /// Generates a random code for various purposes
    /// </summary>
    /// <param name="length">Length of the code</param>
    /// <param name="alphanumeric">Whether to include letters (true) or numbers only (false)</param>
    /// <returns>Generated code</returns>
    public static string GenerateRandomCode(int length = 6, bool alphanumeric = false)
    {
        return alphanumeric
            ? Auth.AuthGeneratorHelper.GenerateActivationCode(length)
            : GenerateNumericToken(length);
    }

    #endregion

    #region Private Helpers

    /// <summary>
    /// Generates a random number with specified digit count
    /// For 4 digits: 1000-9999, For 3 digits: 100-999, etc.
    /// </summary>
    private static int GenerateRandomNumber(int digitCount)
    {
        if (digitCount < 1)
            throw new ArgumentException("Digit count must be at least 1", nameof(digitCount));

        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        var randomBytes = new byte[4];
        rng.GetBytes(randomBytes);
        var baseNumber = Math.Abs(BitConverter.ToInt32(randomBytes, 0));

        var min = (int)Math.Pow(10, digitCount - 1);
        var max = (int)Math.Pow(10, digitCount) - 1;

        return baseNumber % (max - min + 1) + min;
    }

    /// <summary>
    /// Gets the type code for maturity notices
    /// </summary>
    private static string GetNoticeTypeCode(MaturityNoticeType noticeType)
    {
        return noticeType switch
        {
            MaturityNoticeType.Initial => "IN",
            MaturityNoticeType.Reminder => "RM",
            MaturityNoticeType.Final => "FN",
            MaturityNoticeType.AutoRenewal => "AR",
            _ => "GN"
        };
    }

    #endregion
}
