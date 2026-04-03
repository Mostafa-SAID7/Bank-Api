using System.Security.Cryptography;
using System.Text;

namespace Bank.Application.Utilities.Auth;

/// <summary>
/// Utility for generating random passwords
/// </summary>
public static class PasswordGenerator
{
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
}
