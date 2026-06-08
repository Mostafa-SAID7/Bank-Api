using Bank.Application.DTOs;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service for core fixed deposit operations (creation, retrieval, queries)
/// </summary>
public interface IFixedDepositService
{
    Task<FixedDepositDto> CreateFixedDepositAsync(CreateFixedDepositRequest request, Guid customerId);
    Task<FixedDepositDto?> GetFixedDepositAsync(Guid depositId);
    Task<FixedDepositDto?> GetFixedDepositByNumberAsync(string depositNumber);
    Task<IEnumerable<FixedDepositDto>> GetCustomerFixedDepositsAsync(Guid customerId);
    Task<IEnumerable<FixedDepositDto>> GetMaturingDepositsAsync(DateTime fromDate, DateTime toDate);
}
