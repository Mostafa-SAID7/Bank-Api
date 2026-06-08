using Bank.Application.DTOs;
using Bank.Domain.Entities;

namespace Bank.Application.Helpers.Deposit;

/// <summary>
/// Centralized mapping helper for Deposit domain DTOs
/// Eliminates duplication across DepositService, DepositProductService, InterestTierService, and FixedDepositService
/// </summary>
public static class DepositMappingHelper
{
    /// <summary>
    /// Maps DepositProduct entity to DTO
    /// </summary>
    public static DepositProductDto MapToDepositProductDto(DepositProduct product)
    {
        return new DepositProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ProductType = product.ProductType,
            IsActive = product.IsActive,
            MinimumTermDays = product.MinimumTermDays,
            MaximumTermDays = product.MaximumTermDays,
            DefaultTermDays = product.DefaultTermDays,
            MinimumBalance = product.MinimumBalance,
            MaximumBalance = product.MaximumBalance,
            MinimumOpeningBalance = product.MinimumOpeningBalance,
            BaseInterestRate = product.BaseInterestRate,
            InterestCalculationMethod = product.InterestCalculationMethod,
            CompoundingFrequency = product.CompoundingFrequency,
            HasTieredRates = product.HasTieredRates,
            AllowPartialWithdrawals = product.AllowPartialWithdrawals,
            PenaltyType = product.PenaltyType,
            PenaltyAmount = product.PenaltyAmount,
            PenaltyPercentage = product.PenaltyPercentage,
            PenaltyFreeDays = product.PenaltyFreeDays,
            DefaultMaturityAction = product.DefaultMaturityAction,
            AllowAutoRenewal = product.AllowAutoRenewal,
            AutoRenewalNoticeDays = product.AutoRenewalNoticeDays,
            PromotionalRateStartDate = product.PromotionalRateStartDate,
            PromotionalRateEndDate = product.PromotionalRateEndDate,
            PromotionalRate = product.PromotionalRate,
            IsPromotionalRateActive = product.IsPromotionalRateActive(),
            InterestTiers = product.InterestTiers.Select(MapToInterestTierDto).ToList(),
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }

    /// <summary>
    /// Maps InterestTier entity to DTO
    /// </summary>
    public static InterestTierDto MapToInterestTierDto(InterestTier tier)
    {
        return new InterestTierDto
        {
            Id = tier.Id,
            DepositProductId = tier.DepositProductId,
            TierName = tier.TierName,
            MinimumBalance = tier.MinimumBalance,
            MaximumBalance = tier.MaximumBalance,
            InterestRate = tier.InterestRate,
            TierBasis = tier.TierBasis,
            IsActive = tier.IsActive,
            DisplayOrder = tier.DisplayOrder,
            EffectiveFromDate = tier.EffectiveFromDate,
            EffectiveToDate = tier.EffectiveToDate,
            IsPromotional = tier.IsPromotional,
            IsEffective = tier.IsEffective()
        };
    }

    /// <summary>
    /// Maps FixedDeposit entity to DTO (async version for loading related entities)
    /// </summary>
    public static async Task<FixedDepositDto> MapToFixedDepositDtoAsync(
        FixedDeposit deposit,
        Func<Guid, Task<FixedDeposit>>? loadEntityFunc = null)
    {
        // Load related entities if not already loaded
        if (deposit.Customer == null && loadEntityFunc != null)
        {
            deposit = await loadEntityFunc(deposit.Id);
        }

        var daysToMaturity = (deposit.MaturityDate - DateTime.UtcNow).Days;

        return new FixedDepositDto
        {
            Id = deposit.Id,
            DepositNumber = deposit.DepositNumber,
            CustomerId = deposit.CustomerId,
            CustomerName = deposit.Customer?.UserName ?? string.Empty,
            DepositProductId = deposit.DepositProductId,
            ProductName = deposit.DepositProduct?.Name ?? string.Empty,
            LinkedAccountId = deposit.LinkedAccountId,
            LinkedAccountNumber = deposit.LinkedAccount?.AccountNumber ?? string.Empty,
            PrincipalAmount = deposit.PrincipalAmount,
            InterestRate = deposit.InterestRate,
            TermDays = deposit.TermDays,
            StartDate = deposit.StartDate,
            MaturityDate = deposit.MaturityDate,
            Status = deposit.Status,
            InterestCalculationMethod = deposit.InterestCalculationMethod,
            CompoundingFrequency = deposit.CompoundingFrequency,
            AccruedInterest = deposit.AccruedInterest,
            LastInterestCalculationDate = deposit.LastInterestCalculationDate,
            MaturityAction = deposit.MaturityAction,
            AutoRenewalEnabled = deposit.AutoRenewalEnabled,
            RenewalTermDays = deposit.RenewalTermDays,
            RenewalNoticeDate = deposit.RenewalNoticeDate,
            CustomerConsentReceived = deposit.CustomerConsentReceived,
            PenaltyType = deposit.PenaltyType,
            PenaltyAmount = deposit.PenaltyAmount,
            PenaltyPercentage = deposit.PenaltyPercentage,
            ClosureDate = deposit.ClosureDate,
            ClosureReason = deposit.ClosureReason,
            PenaltyApplied = deposit.PenaltyApplied,
            NetAmountPaid = deposit.NetAmountPaid,
            RenewalCount = deposit.RenewalCount,
            MaturityAmount = deposit.CalculateMaturityAmount(),
            InterestAtMaturity = deposit.CalculateInterestAtMaturity(),
            DaysToMaturity = Math.Max(0, daysToMaturity),
            HasMatured = deposit.HasMatured(),
            CreatedAt = deposit.CreatedAt,
            UpdatedAt = deposit.UpdatedAt
        };
    }

    /// <summary>
    /// Maps DepositCertificate entity to DTO
    /// </summary>
    public static DepositCertificateDto MapToDepositCertificateDto(DepositCertificate certificate)
    {
        return new DepositCertificateDto
        {
            Id = certificate.Id,
            FixedDepositId = certificate.FixedDepositId,
            CertificateNumber = certificate.CertificateNumber,
            Status = certificate.Status,
            IssueDate = certificate.IssueDate,
            DeliveryDate = certificate.DeliveryDate,
            DeliveryMethod = certificate.DeliveryMethod,
            DeliveryAddress = certificate.DeliveryAddress,
            DeliveryReference = certificate.DeliveryReference,
            PdfFileName = certificate.PdfFileName,
            HasPdf = certificate.CertificatePdf != null
        };
    }

    /// <summary>
    /// Maps MaturityNotice entity to DTO
    /// </summary>
    public static MaturityNoticeDto MapToMaturityNoticeDto(MaturityNotice notice)
    {
        return new MaturityNoticeDto
        {
            Id = notice.Id,
            FixedDepositId = notice.FixedDepositId,
            NoticeNumber = notice.NoticeNumber,
            NoticeType = notice.NoticeType,
            NoticeDate = notice.NoticeDate,
            MaturityDate = notice.MaturityDate,
            Status = notice.Status,
            Subject = notice.Subject,
            DeliveryChannel = notice.DeliveryChannel,
            DeliveryAddress = notice.DeliveryAddress,
            DeliveryDate = notice.DeliveryDate,
            DeliveryAttempts = notice.DeliveryAttempts,
            CustomerResponseDate = notice.CustomerResponseDate,
            CustomerChoice = notice.CustomerChoice,
            CustomerInstructions = notice.CustomerInstructions,
            ConsentReceived = notice.ConsentReceived
        };
    }
}
