using System.Security.Cryptography;

namespace Bank.Application.Helpers.Auth;

/// <summary>
/// Utility for generating secure tokens and codes
/// </summary>
public static class SecureTokenGenerator
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
        var result = new System.Text.StringBuilder(length);
        var buffer = new byte[4];

        for (int i = 0; i < length; i++)
        {
            rng.GetBytes(buffer);
            var randomIndex = Math.Abs(BitConverter.ToInt32(buffer, 0)) % chars.Length;
            result.Append(chars[randomIndex]);
        }

        return result.ToString();
    }
}
