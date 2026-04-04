using System;
using Bank.Domain.Enums;

namespace Bank.Application.Helpers.Shared
{
    /// <summary>
    /// Helper for financial calculations
    /// </summary>
    public static class CalculationHelper
    {
        /// <summary>
        /// Calculate processing fee for a bill payment
        /// </summary>
        public static decimal CalculateProcessingFee(decimal amount, PaymentMethod method)
        {
            return method switch
            {
                PaymentMethod.BankTransfer => 0.50m,
                PaymentMethod.DebitCard => amount * 0.015m,
                PaymentMethod.CreditCard => amount * 0.025m,
                _ => 1.00m
            };
        }

        /// <summary>
        /// Calculate monthly payment for a loan
        /// </summary>
        public static decimal CalculateMonthlyPayment(decimal principal, decimal annualRate, int months)
        {
            if (months <= 0 || annualRate <= 0) return principal;

            double monthlyRate = (double)annualRate / 12 / 100;
            double payment = (double)principal * (monthlyRate * Math.Pow(1 + monthlyRate, months)) / (Math.Pow(1 + monthlyRate, months) - 1);

            return Math.Round((decimal)payment, 2);
        }

        /// <summary>
        /// Calculate simple interest
        /// </summary>
        public static decimal CalculateSimpleInterest(decimal principal, decimal annualRate, int days)
        {
            return Math.Round(principal * (annualRate / 100) * days / 365, 2);
        }

        /// <summary>
        /// Calculate compound interest
        /// </summary>
        public static decimal CalculateCompoundInterest(decimal principal, decimal annualRate, int days, int compoundsPerYear)
        {
            if (compoundsPerYear <= 0) return CalculateSimpleInterest(principal, annualRate, days);
            
            double years = (double)days / 365;
            double rate = (double)annualRate / 100;
            double amount = (double)principal * Math.Pow(1 + (rate / compoundsPerYear), compoundsPerYear * years);
            
            return Math.Round((decimal)amount - principal, 2);
        }

        /// <summary>
        /// Calculate default interest rate for a product type
        /// </summary>
        public static decimal CalculateDefaultRate(DepositProductType type)
        {
            return type switch
            {
                DepositProductType.SavingsAccount => 3.5m,
                DepositProductType.FixedDeposit => 5.25m,
                DepositProductType.RecurringDeposit => 4.75m,
                _ => 2.0m
            };
        }

        /// <summary>
        /// Calculate principal and interest allocations for a payment
        /// </summary>
        public static (decimal principalAmount, decimal interestAmount) CalculatePaymentAllocation(decimal outstandingBalance, decimal paymentAmount, decimal monthlyInterestRate)
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
    }
}
