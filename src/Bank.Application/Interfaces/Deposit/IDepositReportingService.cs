using Bank.Application.DTOs;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service for deposit reporting and analytics
/// </summary>
public interface IDepositReportingService
{
    Task<DepositSummaryDto> GetDepositSummaryAsync(Guid customerId);
    Task<IEnumerable<DepositTransactionDto>> GetDepositTransactionsAsync(Guid depositId, DateTime? fromDate = null, DateTime? toDate = null);
    Task<DepositPortfolioDto> GetCustomerDepositPortfolioAsync(Guid customerId);
}
