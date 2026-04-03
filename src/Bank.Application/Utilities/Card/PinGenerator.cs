namespace Bank.Application.Utilities.Card;

/// <summary>
/// Utility for generating card PINs
/// </summary>
public static class PinGenerator
{
    /// <summary>
    /// Generates a random PIN
    /// </summary>
    /// <param name="length">Length of the PIN (default 4)</param>
    /// <returns>Numeric PIN as string</returns>
    public static string GenerateRandomPin(int length = 4)
    {
        return Auth.SecureTokenGenerator.GenerateNumericToken(length);
    }
}
