namespace Bank.Application.Helpers.Shared;

/// <summary>
/// Utility for generating random codes for various purposes
/// </summary>
public static class RandomCodeGenerator
{
    /// <summary>
    /// Generates a random code for various purposes
    /// </summary>
    /// <param name="length">Length of the code</param>
    /// <param name="alphanumeric">Whether to include letters (true) or numbers only (false)</param>
    /// <returns>Generated code</returns>
    public static string GenerateRandomCode(int length = 6, bool alphanumeric = false)
    {
        return alphanumeric 
            ? Auth.SecureTokenGenerator.GenerateActivationCode(length) 
            : Auth.SecureTokenGenerator.GenerateNumericToken(length);
    }
}
