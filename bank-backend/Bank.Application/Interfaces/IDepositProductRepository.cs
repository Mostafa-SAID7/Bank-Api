using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;

namespace Bank.Application.Interfaces;

/// <summary>
/// Repository interface for DepositProduct entity
/// </summary>
public interface IDepositProductRepository : IRepository<DepositProduct>
{
    Task<IEnumerable<DepositProduct>> GetActiveProductsAsync();
    Task<IEnumerable<DepositProduct>> GetProductsByTypeAsync(DepositProductType productType);
    Task<DepositProduct?> GetProductWithTiersAsync(Guid productId);
    Task<IEnumerable<DepositProduct>> GetProductsWithPromotionalRatesAsync();
}

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

/// <summary>
/// Repository interface for InterestTier entity
/// </summary>
public interface IInterestTierRepository : IRepository<InterestTier>
{
    Task<IEnumerable<InterestTier>> GetTiersByProductAsync(Guid productId);
    Task<IEnumerable<InterestTier>> GetActiveTiersAsync(Guid productId);
    Task<InterestTier?> GetApplicableTierAsync(Guid productId, decimal balance);
}

/// <summary>
/// Repository interface for DepositTransaction entity
/// </summary>
public interface IDepositTransactionRepository : IRepository<DepositTransaction>
{
    Task<IEnumerable<DepositTransaction>> GetDepositTransactionsAsync(Guid depositId);
    Task<IEnumerable<DepositTransaction>> GetTransactionsByTypeAsync(Guid depositId, DepositTransactionType transactionType);
    Task<IEnumerable<DepositTransaction>> GetTransactionsByDateRangeAsync(Guid depositId, DateTime fromDate, DateTime toDate);
    Task<decimal> GetTotalInterestCreditedAsync(Guid depositId);
}

/// <summary>
/// Repository interface for DepositCertificate entity
/// </summary>
public interface IDepositCertificateRepository : IRepository<DepositCertificate>
{
    Task<IEnumerable<DepositCertificate>> GetCertificatesByDepositAsync(Guid depositId);
    Task<DepositCertificate?> GetByCertificateNumberAsync(string certificateNumber);
    Task<IEnumerable<DepositCertificate>> GetPendingDeliveryAsync();
}

/// <summary>
/// Repository interface for MaturityNotice entity
/// </summary>
public interface IMaturityNoticeRepository : IRepository<MaturityNotice>
{
    Task<IEnumerable<MaturityNotice>> GetNoticesByDepositAsync(Guid depositId);
    Task<IEnumerable<MaturityNotice>> GetPendingNoticesAsync();
    Task<IEnumerable<MaturityNotice>> GetNoticesByTypeAsync(MaturityNoticeType noticeType);
    Task<MaturityNotice?> GetByNoticeNumberAsync(string noticeNumber);
}