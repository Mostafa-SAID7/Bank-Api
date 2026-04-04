using Bank.Application.Helpers.Auth;

namespace Bank.Application.Helpers.Shared;

/// <summary>
/// Helper for generating secure tokens
/// </summary>
public static class TokenGenerationHelper
{
    /// <summary>
    /// Generates a secure random token
    /// </summary>
    public static string GenerateSecureToken(int length = 32)
    {
        return SecureTokenGenerator.GenerateSecureToken(length);
    }

    /// <summary>
    /// Generates a numeric token code
    /// </summary>
    public static string GenerateNumericToken(int length = 6)
    {
        return SecureTokenGenerator.GenerateNumericToken(length);
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
}
