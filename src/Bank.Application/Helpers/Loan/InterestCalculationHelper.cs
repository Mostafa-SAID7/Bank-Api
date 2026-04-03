namespace Bank.Application.Helpers.Loan;

/// <summary>
/// Utility for loan interest calculations
/// </summary>
public static class InterestCalculationHelper
{
    /// <summary>
    /// Calculates compound interest
    /// </summary>
    /// <param name="principal">Principal amount</param>
    /// <param name="rate">Annual interest rate (as decimal, e.g., 0.05 for 5%)</param>
    /// <param name="compoundingFrequency">Number of times interest is compounded per year</param>
    /// <param name="time">Time in years</param>
    /// <returns>Compound interest amount</returns>
    public static decimal CalculateCompoundInterest(decimal principal, decimal rate, int compoundingFrequency, decimal time)
    {
        if (principal <= 0 || rate < 0 || compoundingFrequency <= 0 || time < 0)
            return 0;

        var ratePerPeriod = rate / compoundingFrequency;
        var numberOfPeriods = compoundingFrequency * time;
        
        var compoundAmount = principal * (decimal)Math.Pow((double)(1 + ratePerPeriod), (double)numberOfPeriods);
        return compoundAmount - principal;
    }

    /// <summary>
    /// Calculates simple interest
    /// </summary>
    /// <param name="principal">Principal amount</param>
    /// <param name="rate">Annual interest rate (as decimal)</param>
    /// <param name="time">Time in years</param>
    /// <returns>Simple interest amount</returns>
    public static decimal CalculateSimpleInterest(decimal principal, decimal rate, decimal time)
    {
        if (principal <= 0 || rate < 0 || time < 0)
            return 0;

        return principal * rate * time;
    }

    /// <summary>
    /// Calculates interest rate from credit score
    /// </summary>
    /// <param name="creditScore">Credit score (300-850)</param>
    /// <param name="baseRate">Base interest rate</param>
    /// <param name="maxRate">Maximum interest rate</param>
    /// <returns>Calculated interest rate</returns>
    public static decimal CalculateInterestRateFromScore(int creditScore, decimal baseRate = 0.03m, decimal maxRate = 0.25m)
    {
        // Normalize credit score to 0-1 range (300-850 -> 0-1)
        var normalizedScore = Math.Max(0, Math.Min(1, (creditScore - 300m) / 550m));
        
        // Higher score = lower rate
        var rateMultiplier = 1 - normalizedScore;
        var calculatedRate = baseRate + (maxRate - baseRate) * rateMultiplier;
        
        return Math.Max(baseRate, Math.Min(maxRate, calculatedRate));
    }
}
