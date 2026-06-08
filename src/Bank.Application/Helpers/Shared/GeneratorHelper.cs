using System.Security.Cryptography;
using Bank.Domain.Enums;

namespace Bank.Application.Helpers.Shared;

/// <summary>
/// Centralized helper for generating unique identifiers, numbers, and codes
/// </summary>
public static class GeneratorHelper
{
    /// <summary>
    /// Generates a unique fixed deposit number (FD + YYYYMMDD + 4-digit random)
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
    /// Generates a random number with specified digit count
    /// For 4 digits: 1000-9999, For 3 digits: 100-999, etc.
    /// </summary>
    private static int GenerateRandomNumber(int digitCount)
    {
        if (digitCount < 1)
            throw new ArgumentException("Digit count must be at least 1", nameof(digitCount));

        using var rng = RandomNumberGenerator.Create();
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
}
