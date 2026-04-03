namespace Bank.Application.Validators.Shared;

/// <summary>
/// Validator for US routing number format and checksum
/// </summary>
public static class RoutingNumberValidator
{
    /// <summary>
    /// Validates US routing number checksum
    /// </summary>
    /// <param name="routingNumber">9-digit routing number</param>
    /// <returns>True if routing number is valid</returns>
    public static bool ValidateChecksum(string routingNumber)
    {
        if (string.IsNullOrWhiteSpace(routingNumber) || routingNumber.Length != 9)
            return false;

        if (!routingNumber.All(char.IsDigit))
            return false;

        var digits = routingNumber.Select(c => int.Parse(c.ToString())).ToArray();

        // ABA routing number checksum algorithm
        var checksum = (3 * (digits[0] + digits[3] + digits[6]) +
                       7 * (digits[1] + digits[4] + digits[7]) +
                       (digits[2] + digits[5] + digits[8])) % 10;

        return checksum == 0;
    }
}
