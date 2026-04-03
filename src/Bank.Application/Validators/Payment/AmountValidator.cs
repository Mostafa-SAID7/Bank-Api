namespace Bank.Application.Validators.Payment;

/// <summary>
/// Validator for payment amount range and format
/// </summary>
public static class AmountValidator
{
    /// <summary>
    /// Validates amount is within acceptable range
    /// </summary>
    /// <param name="amount">Amount to validate</param>
    /// <param name="minAmount">Minimum allowed amount</param>
    /// <param name="maxAmount">Maximum allowed amount</param>
    /// <returns>True if amount is valid</returns>
    public static bool ValidateRange(decimal amount, decimal minAmount = 0.01m, decimal? maxAmount = null)
    {
        if (amount < minAmount)
            return false;

        if (maxAmount.HasValue && amount > maxAmount.Value)
            return false;

        return true;
    }
}
