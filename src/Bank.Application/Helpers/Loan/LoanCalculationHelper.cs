namespace Bank.Application.Helpers.Loan;

/// <summary>
/// Consolidated helper for all loan-related calculations including:
/// - Loan amount and payment calculations
/// - Interest calculations (simple and compound)
/// - Risk and delinquency analysis
/// Combines functionality from LoanCalculationHelper, InterestCalculationHelper, 
/// RiskCalculationHelper, and related calculation utilities.
/// </summary>
public static class LoanCalculationHelper
{
    #region Loan Amount and Payment Calculations

    /// <summary>
    /// Calculates maximum loan amount based on income and debt-to-income ratio
    /// </summary>
    /// <param name="monthlyIncome">Monthly income</param>
    /// <param name="existingDebt">Existing monthly debt payments</param>
    /// <param name="maxDebtToIncomeRatio">Maximum allowed debt-to-income ratio</param>
    /// <param name="loanTermMonths">Loan term in months</param>
    /// <param name="interestRate">Annual interest rate</param>
    /// <returns>Maximum loan amount</returns>
    public static decimal CalculateMaxLoanAmount(decimal monthlyIncome, decimal existingDebt, 
        decimal maxDebtToIncomeRatio, int loanTermMonths, decimal interestRate)
    {
        if (monthlyIncome <= 0 || loanTermMonths <= 0 || interestRate < 0)
            return 0;

        var maxTotalDebt = monthlyIncome * maxDebtToIncomeRatio;
        var availableForNewDebt = maxTotalDebt - existingDebt;
        
        if (availableForNewDebt <= 0)
            return 0;

        // Calculate loan amount using payment formula: P = PMT * [(1 - (1 + r)^-n) / r]
        var monthlyRate = interestRate / 12;
        if (monthlyRate == 0)
            return availableForNewDebt * loanTermMonths;

        var denominator = monthlyRate * (decimal)Math.Pow((double)(1 + monthlyRate), loanTermMonths);
        var numerator = denominator - monthlyRate;
        
        return availableForNewDebt * (numerator / denominator);
    }

    /// <summary>
    /// Calculates monthly payment for a loan
    /// </summary>
    /// <param name="principal">Loan principal</param>
    /// <param name="annualRate">Annual interest rate</param>
    /// <param name="termMonths">Loan term in months</param>
    /// <returns>Monthly payment amount</returns>
    public static decimal CalculateMonthlyPayment(decimal principal, decimal annualRate, int termMonths)
    {
        if (principal <= 0 || termMonths <= 0)
            return 0;

        if (annualRate == 0)
            return principal / termMonths;

        var monthlyRate = annualRate / 12;
        var denominator = 1 - (decimal)Math.Pow((double)(1 + monthlyRate), -termMonths);
        
        return principal * (monthlyRate / denominator);
    }

    #endregion

    #region Interest Calculations

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

    #endregion

    #region Risk and Delinquency Analysis

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

    #endregion
}
