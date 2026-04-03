using System.Security.Cryptography;

namespace Bank.Application.Utilities.Payment;

/// <summary>
/// Utility for generating payment confirmation numbers and external references
/// </summary>
public static class ConfirmationNumberGenerator
{
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
    /// Generates an external reference for payment processing
    /// </summary>
    /// <returns>External reference string</returns>
    public static string GenerateExternalReference()
    {
        return $"EXT-{Guid.NewGuid():N}"[..16];
    }
}
