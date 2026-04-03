using System.Security.Cryptography;
using System.Text;

namespace Bank.Application.Utilities;

/// <summary>
/// Centralized helper for generating tokens, codes, and secure random values
/// </summary>
public static class TokenGenerationHelper
{
    /// <summary>
    /// Generates a secure random token of specified length
    /// </summary>
    /// <param name="length">Length of the token</param>
    /// <returns>Base64 encoded secure token</returns>
    public static string GenerateSecureToken(int length = 32)
    {
        using var rng = RandomNumberGenerator.Create();
        var tokenBytes = new byte[length];
        rng.GetBytes(tokenBytes);
        return Convert.ToBase64String(tokenBytes);
    }

    /// <summary>
    /// Generates a numeric token of specified length
    /// </summary>
    /// <param name="length">Length of the numeric token</param>
    /// <returns>Numeric token as string</returns>
    public static string GenerateNumericToken(int length = 6)
    {
        using var rng = RandomNumberGenerator.Create();
        var tokenBytes = new byte[4];
        rng.GetBytes(tokenBytes);
        var number = Math.Abs(BitConverter.ToInt32(tokenBytes, 0));
        var token = (number % (int)Math.Pow(10, length)).ToString().PadLeft(length, '0');
        return token;
    }

    /// <summary>
    /// Generates an activation code
    /// </summary>
    /// <param name="length">Length of the activation code</param>
    /// <returns>Alphanumeric activation code</returns>
    public static string GenerateActivationCode(int length = 8)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        using var rng = RandomNumberGenerator.Create();
        var result = new StringBuilder(length);
        var buffer = new byte[4];

        for (int i = 0; i < length; i++)
        {
            rng.GetBytes(buffer);
            var randomIndex = Math.Abs(BitConverter.ToInt32(buffer, 0)) % chars.Length;
            result.Append(chars[randomIndex]);
        }

        return result.ToString();
    }

    /// <summary>
    /// Generates a random PIN
    /// </summary>
    /// <param name="length">Length of the PIN (default 4)</param>
    /// <returns>Numeric PIN as string</returns>
    public static string GenerateRandomPin(int length = 4)
    {
        return GenerateNumericToken(length);
    }

    /// <summary>
    /// Generates a confirmation number with timestamp
    /// </summary>
    /// <param name="prefix">Optional prefix for the confirmation number</param>
    /// <returns>Confirmation number</returns>
    public static string GenerateConfirmationNumber(string prefix = "CNF")
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[4];
        rng.GetBytes(randomBytes);
        var random = Math.Abs(BitConverter.ToInt32(randomBytes, 0)) % 900000 + 100000;
        return $"{prefix}{timestamp}{random}";
    }

    /// <summary>
    /// Generates backup codes for two-factor authentication
    /// </summary>
    /// <param name="count">Number of backup codes to generate</param>
    /// <param name="length">Length of each backup code</param>
    /// <returns>List of backup codes</returns>
    public static List<string> GenerateBackupCodes(int count = 10, int length = 8)
    {
        var codes = new List<string>();
        for (int i = 0; i < count; i++)
        {
            codes.Add(GenerateActivationCode(length));
        }
        return codes;
    }

    /// <summary>
    /// Generates a secret key for TOTP
    /// </summary>
    /// <param name="length">Length of the secret key in bytes</param>
    /// <returns>Base32 encoded secret key</returns>
    public static string GenerateSecretKey(int length = 20)
    {
        using var rng = RandomNumberGenerator.Create();
        var keyBytes = new byte[length];
        rng.GetBytes(keyBytes);
        return Convert.ToBase64String(keyBytes);
    }

    /// <summary>
    /// Generates a QR code URL for TOTP setup
    /// </summary>
    /// <param name="issuer">The issuer name (e.g., "Bank App")</param>
    /// <param name="accountName">The account name (e.g., user email)</param>
    /// <param name="secretKey">The secret key</param>
    /// <returns>TOTP URL for QR code generation</returns>
    public static string GenerateQrCodeUrl(string issuer, string accountName, string secretKey)
    {
        var encodedIssuer = Uri.EscapeDataString(issuer);
        var encodedAccountName = Uri.EscapeDataString(accountName);
        var encodedSecret = Uri.EscapeDataString(secretKey);
        
        return $"otpauth://totp/{encodedIssuer}:{encodedAccountName}?secret={encodedSecret}&issuer={encodedIssuer}";
    }

    /// <summary>
    /// Generates a random password with specified criteria
    /// </summary>
    /// <param name="length">Password length</param>
    /// <param name="includeUppercase">Include uppercase letters</param>
    /// <param name="includeLowercase">Include lowercase letters</param>
    /// <param name="includeNumbers">Include numbers</param>
    /// <param name="includeSpecialChars">Include special characters</param>
    /// <returns>Generated password</returns>
    public static string GeneratePassword(int length = 12, bool includeUppercase = true, 
        bool includeLowercase = true, bool includeNumbers = true, bool includeSpecialChars = true)
    {
        var chars = new StringBuilder();
        if (includeUppercase) chars.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        if (includeLowercase) chars.Append("abcdefghijklmnopqrstuvwxyz");
        if (includeNumbers) chars.Append("0123456789");
        if (includeSpecialChars) chars.Append("!@#$%^&*()_+-=[]{}|;:,.<>?");

        if (chars.Length == 0)
            throw new ArgumentException("At least one character type must be included");

        using var rng = RandomNumberGenerator.Create();
        var result = new StringBuilder(length);
        var buffer = new byte[4];

        for (int i = 0; i < length; i++)
        {
            rng.GetBytes(buffer);
            var randomIndex = Math.Abs(BitConverter.ToInt32(buffer, 0)) % chars.Length;
            result.Append(chars[randomIndex]);
        }

        return result.ToString();
    }

    /// <summary>
    /// Generates an external reference for payment processing
    /// </summary>
    /// <returns>External reference string</returns>
    public static string GenerateExternalReference()
    {
        return $"EXT-{Guid.NewGuid():N}"[..16];
    }

    /// <summary>
    /// Generates a random code for various purposes
    /// </summary>
    /// <param name="length">Length of the code</param>
    /// <param name="alphanumeric">Whether to include letters (true) or numbers only (false)</param>
    /// <returns>Generated code</returns>
    public static string GenerateRandomCode(int length = 6, bool alphanumeric = false)
    {
        return alphanumeric ? GenerateActivationCode(length) : GenerateNumericToken(length);
    }
}