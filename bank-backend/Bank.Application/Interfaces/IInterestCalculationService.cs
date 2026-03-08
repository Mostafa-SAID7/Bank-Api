using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

public interface IInterestCalculationService
{
    Task<decimal> CalculateSimpleInterestAsync(Account account, DateTime fromDate, DateTime toDate);
    Task<decimal> CalculateCompoundInterestAsync(Account account, DateTime fromDate, DateTime toDate, int compoundingFrequency = 12);
    Task<decimal> CalculateDailyInterestAsync(Account account, DateTime date);
    Task<bool> ApplyInterestAsync(int accountId, int userId);
    Task<bool> ProcessMonthlyInterestAsync();
    Task<List<Account>> GetAccountsForInterestProcessingAsync();
    Task<decimal> GetInterestRateAsync(AccountType accountType, decimal balance);
    Task<bool> UpdateInterestRateAsync(int accountId, decimal newRate, int userId);
}