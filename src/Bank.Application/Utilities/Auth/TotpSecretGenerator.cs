using System.Security.Cryptography;

namespace Bank.Application.Utilities.Auth;

/// <summary>
/// Utility for generating TOTP (Time-based One-Time Password) secrets and QR codes
/// </summary>
public static class TotpSecretGenerator
{
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
}
