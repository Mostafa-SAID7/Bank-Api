namespace Bank.Application.Helpers.Auth;

/// <summary>
/// Utility for generating backup codes for two-factor authentication
/// </summary>
public static class BackupCodeGenerator
{
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
            codes.Add(SecureTokenGenerator.GenerateActivationCode(length));
        }
        return codes;
    }
}
