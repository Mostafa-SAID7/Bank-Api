using Bank.Domain.Enums;

namespace Bank.Application.Helpers.Payment;

/// <summary>
/// Consolidated helper for payment calculations and fee allocations.
/// Combines functionality from PaymentCalculationHelper and CalculationHelper payment methods.
/// </summary>
public static class PaymentCalculationHelper
{
    #region Payment Allocation

    /// <summary>
    /// Calculates loan payment allocation between principal and interest
    /// </summary>
    /// <param name="remainingBalance">Remaining loan balance</param>
    /// <param name="monthlyPayment">Monthly payment amount</param>
    /// <param name="monthlyInterestRate">Monthly interest rate</param>
    /// <returns>Tuple of (principal payment, interest payment)</returns>
    public static (decimal PrincipalPayment, decimal InterestPayment) CalculatePaymentAllocation(
        decimal remainingBalance, decimal monthlyPayment, decimal monthlyInterestRate)
    {
        if (remainingBalance <= 0 || monthlyPayment <= 0)
            return (0, 0);

        var interestPayment = remainingBalance * monthlyInterestRate;
        var principalPayment = Math.Max(0, monthlyPayment - interestPayment);
        
        // Ensure principal payment doesn't exceed remaining balance
        principalPayment = Math.Min(principalPayment, remainingBalance);
        
        return (principalPayment, interestPayment);
    }

    /// <summary>
    /// Calculates principal and interest allocations for a payment (alternative method)
    /// </summary>
    public static (decimal principalAmount, decimal interestAmount) CalculatePaymentComponents(
        decimal outstandingBalance, decimal paymentAmount, decimal monthlyInterestRate)
    {
        var interestAmount = Math.Round(outstandingBalance * monthlyInterestRate, 2);
        var principalAmount = paymentAmount - interestAmount;
        if (principalAmount < 0)
        {
            principalAmount = 0;
            interestAmount = paymentAmount;
        }
        
        return (principalAmount, interestAmount);
    }

    #endregion

    #region Fee Calculations

    /// <summary>
    /// Calculates processing fee based on amount and payment method
    /// </summary>
    /// <param name="amount">Transaction amount</param>
    /// <param name="paymentMethod">Payment method</param>
    /// <returns>Processing fee</returns>
    public static decimal CalculateProcessingFee(decimal amount, PaymentMethod paymentMethod)
    {
        return paymentMethod switch
        {
            PaymentMethod.ACH => Math.Max(0.50m, amount * 0.001m),
            PaymentMethod.WireTransfer => 15.00m,
            PaymentMethod.Check => 2.50m,
            PaymentMethod.BankTransfer => Math.Max(1.00m, amount * 0.002m),
            PaymentMethod.CreditCard => amount * 0.025m,
            PaymentMethod.DebitCard => amount * 0.015m,
            _ => 1.00m
        };
    }

    #endregion
}
