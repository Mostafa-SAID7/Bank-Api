using Bank.Domain.Entities;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for FixedDeposit entity
/// </summary>
public interface IFixedDepositRepository : IRepository<FixedDeposit>
{
    Task<FixedDeposit?> GetByDepositNumberAsync(string depositNumber);
    Task<IEnumerable<FixedDeposit>> GetCustomerDepositsAsync(Guid customerId);
    Task<IEnumerable<FixedDeposit>> GetMaturingDepositsAsync(DateTime fromDate, DateTime toDate);
    Task<IEnumerable<FixedDeposit>> GetActiveDepositsAsync();
    Task<IEnumerable<FixedDeposit>> GetDepositsForInterestProcessingAsync();
    Task<IEnumerable<FixedDeposit>> GetDepositsForAutoRenewalAsync();
    Task<FixedDeposit?> GetDepositWithDetailsAsync(Guid depositId);
}
