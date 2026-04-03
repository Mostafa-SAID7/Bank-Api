using Bank.Application.DTOs;
using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for managing deposit products and fixed deposits
/// </summary>
public interface IDepositService
{
    // Deposit Product Management
    Task<DepositProductDto?> GetDepositProductAsync(Guid productId);
    Task<IEnumerable<DepositProductDto>> GetActiveDepositProductsAsync();
    Task<IEnumerable<DepositProductDto>> GetDepositProductsByTypeAsync(DepositProductType productType);
    Task<DepositProductDto> CreateDepositProductAsync(CreateDepositProductRequest request, Guid createdByUserId);
    Task<DepositProductDto> UpdateDepositProductAsync(Guid productId, UpdateDepositProductRequest request, Guid updatedByUserId);
    Task<bool> DeactivateDepositProductAsync(Guid productId, Guid deactivatedByUserId);
    
    // Interest Tier Management
    Task<InterestTierDto> CreateInterestTierAsync(Guid productId, CreateInterestTierRequest request, Guid createdByUserId);
    Task<InterestTierDto> UpdateInterestTierAsync(Guid tierId, UpdateInterestTierRequest request, Guid updatedByUserId);
    Task<bool> DeleteInterestTierAsync(Guid tierId, Guid deletedByUserId);
    Task<IEnumerable<InterestTierDto>> GetInterestTiersAsync(Guid productId);
    
    // Fixed Deposit Management
    Task<FixedDepositDto> CreateFixedDepositAsync(CreateFixedDepositRequest request, Guid customerId);
    Task<FixedDepositDto?> GetFixedDepositAsync(Guid depositId);
    Task<FixedDepositDto?> GetFixedDepositByNumberAsync(string depositNumber);
    Task<IEnumerable<FixedDepositDto>> GetCustomerFixedDepositsAsync(Guid customerId);
    Task<IEnumerable<FixedDepositDto>> GetMaturingDepositsAsync(DateTime fromDate, DateTime toDate);
    
    // Interest Calculation and Processing
    Task<decimal> CalculateInterestAsync(Guid depositId, DateTime fromDate, DateTime toDate);
    Task<bool> ProcessInterestCreditAsync(Guid depositId, Guid processedByUserId);
    Task<bool> ProcessDailyInterestAsync();
    Task<bool> ProcessMonthlyInterestAsync();
    
    // Maturity and Renewal Management
    Task<MaturityDetailsDto> GetMaturityDetailsAsync(Guid depositId);
    Task<bool> ProcessMaturityAsync(Guid depositId, MaturityAction action, Guid processedByUserId);
    Task<FixedDepositDto> RenewFixedDepositAsync(Guid depositId, RenewDepositRequest request, Guid processedByUserId);
    Task<bool> ProcessAutoRenewalsAsync();
    
    // Withdrawal Management
    Task<WithdrawalDetailsDto> CalculateEarlyWithdrawalAsync(Guid depositId, decimal withdrawalAmount);
    Task<bool> ProcessEarlyWithdrawalAsync(Guid depositId, EarlyWithdrawalRequest request, Guid processedByUserId);
    Task<bool> ProcessPartialWithdrawalAsync(Guid depositId, PartialWithdrawalRequest request, Guid processedByUserId);
    
    // Certificate Management
    Task<DepositCertificateDto> GenerateCertificateAsync(Guid depositId, Guid generatedByUserId);
    Task<DepositCertificateDto?> GetCertificateAsync(Guid certificateId);
    Task<byte[]> GetCertificatePdfAsync(Guid certificateId);
    Task<bool> DeliverCertificateAsync(Guid certificateId, string deliveryMethod, string deliveryAddress, Guid deliveredByUserId);
    
    // Notice Management
    Task<MaturityNoticeDto> GenerateMaturityNoticeAsync(Guid depositId, MaturityNoticeType noticeType, Guid generatedByUserId);
    Task<bool> SendMaturityNoticesAsync();
    Task<bool> ProcessCustomerResponseAsync(Guid noticeId, MaturityAction customerChoice, string? instructions, Guid processedByUserId);
    
    // Reporting and Analytics
    Task<DepositSummaryDto> GetDepositSummaryAsync(Guid customerId);
    Task<IEnumerable<DepositTransactionDto>> GetDepositTransactionsAsync(Guid depositId, DateTime? fromDate = null, DateTime? toDate = null);
    Task<DepositPortfolioDto> GetCustomerDepositPortfolioAsync(Guid customerId);
    
    // Background Processing
    Task<bool> ProcessMaturityNoticesAsync();
    Task<bool> ProcessPendingMaturityActionsAsync();
    Task<bool> ProcessInterestAccrualsAsync();
}