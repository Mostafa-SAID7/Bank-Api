using Bank.Domain.Enums;

namespace Bank.Application.Utilities.Deposit;

/// <summary>
/// Utility for deposit penalty calculations
/// </summary>
public static class PenaltyCalculationHelper
{
    /// <summary>
    /// Calculates penalty details for early withdrawal
    /// </summary>
    /// <param name="amount">Withdrawal amount</param>
    /// <param name="penaltyType">Type of penalty</param>
    /// <param name="penaltyAmount">Fixed penalty amount</param>
    /// <param name="penaltyPercentage">Penalty percentage</param>
    /// <returns>Penalty amount</returns>
    public static decimal CalculatePenaltyAmount(decimal amount, WithdrawalPenaltyType penaltyType, 
        decimal? penaltyAmount = null, decimal? penaltyPercentage = null)
    {
        return penaltyType switch
        {
            WithdrawalPenaltyType.FixedAmount => penaltyAmount ?? 0,
            WithdrawalPenaltyType.Percentage => amount * (penaltyPercentage ?? 0) / 100,
            WithdrawalPenaltyType.InterestForfeiture => 0, // Handled separately
            WithdrawalPenaltyType.None => 0,
            _ => 0
        };
    }
}
