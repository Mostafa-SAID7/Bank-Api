namespace Bank.Application.Utilities.Loan;

/// <summary>
/// Utility for loan amount and payment calculations
/// </summary>
public static class LoanCalculationHelper
{
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
}
