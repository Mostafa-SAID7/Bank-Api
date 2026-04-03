namespace Bank.Application.Helpers.Loan;

/// <summary>
/// Utility for loan risk and delinquency calculations
/// </summary>
public static class RiskCalculationHelper
{
    /// <summary>
    /// Calculates delinquency rate
    /// </summary>
    /// <param name="delinquentLoans">Number of delinquent loans</param>
    /// <param name="totalLoans">Total number of loans</param>
    /// <returns>Delinquency rate as percentage</returns>
    public static decimal CalculateDelinquencyRate(int delinquentLoans, int totalLoans)
    {
        if (totalLoans == 0) return 0;
        return (decimal)delinquentLoans / totalLoans * 100;
    }

    /// <summary>
    /// Calculates default rate
    /// </summary>
    /// <param name="defaultedLoans">Number of defaulted loans</param>
    /// <param name="totalLoans">Total number of loans</param>
    /// <returns>Default rate as percentage</returns>
    public static decimal CalculateDefaultRate(int defaultedLoans, int totalLoans)
    {
        if (totalLoans == 0) return 0;
        return (decimal)defaultedLoans / totalLoans * 100;
    }

    /// <summary>
    /// Calculates risk level based on various factors
    /// </summary>
    /// <param name="creditScore">Credit score</param>
    /// <param name="debtToIncomeRatio">Debt-to-income ratio</param>
    /// <param name="loanToValueRatio">Loan-to-value ratio (for secured loans)</param>
    /// <returns>Risk level (Low, Medium, High)</returns>
    public static string CalculateRiskLevel(int creditScore, decimal debtToIncomeRatio, decimal loanToValueRatio = 0)
    {
        var riskScore = 0;

        // Credit score factor (0-3 points)
        if (creditScore >= 750) riskScore += 0;
        else if (creditScore >= 700) riskScore += 1;
        else if (creditScore >= 650) riskScore += 2;
        else riskScore += 3;

        // Debt-to-income ratio factor (0-3 points)
        if (debtToIncomeRatio <= 0.3m) riskScore += 0;
        else if (debtToIncomeRatio <= 0.4m) riskScore += 1;
        else if (debtToIncomeRatio <= 0.5m) riskScore += 2;
        else riskScore += 3;

        // Loan-to-value ratio factor (0-2 points)
        if (loanToValueRatio <= 0.8m) riskScore += 0;
        else if (loanToValueRatio <= 0.9m) riskScore += 1;
        else riskScore += 2;

        return riskScore switch
        {
            <= 2 => "Low",
            <= 5 => "Medium",
            _ => "High"
        };
    }
}
