using Bank.Application.DTOs;
using Bank.Application.Helpers.Shared;
using Bank.Application.Interfaces;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bank.Application.Services;

/// <summary>
/// Facade service for deposit operations
/// Delegates to specialized services to eliminate duplication and maintain single responsibility principle
/// All methods are delegated to appropriate specialized services (Product, Tier, FixedDeposit, Withdrawal, etc.)
/// </summary>
public sealed class DepositService : IDepositService
{
    private readonly IDepositProductService _productService;
    private readonly IInterestTierService _tierService;
    private readonly IFixedDepositService _fixedDepositService;
    private readonly IDepositWithdrawalService _withdrawalService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<DepositService> _logger;

    public DepositService(
        IDepositProductService productService,
        IInterestTierService tierService,
        IFixedDepositService fixedDepositService,
        IDepositWithdrawalService withdrawalService,
        IUnitOfWork unitOfWork,
        IAuditLogService auditLogService,
        ILogger<DepositService> logger)
    {
        _productService = productService;
        _tierService = tierService;
        _fixedDepositService = fixedDepositService;
        _withdrawalService = withdrawalService;
        _unitOfWork = unitOfWork;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    #region Deposit Product Management (Delegates to IDepositProductService)

    public async Task<DepositProductDto?> GetDepositProductAsync(Guid productId)
        => await _productService.GetDepositProductAsync(productId);

    public async Task<IEnumerable<DepositProductDto>> GetActiveDepositProductsAsync()
        => await _productService.GetActiveDepositProductsAsync();

    public async Task<IEnumerable<DepositProductDto>> GetDepositProductsByTypeAsync(DepositProductType productType)
        => await _productService.GetDepositProductsByTypeAsync(productType);

    public async Task<DepositProductDto> CreateDepositProductAsync(CreateDepositProductRequest request, Guid createdByUserId)
        => await _productService.CreateDepositProductAsync(request, createdByUserId);

    public async Task<DepositProductDto> UpdateDepositProductAsync(Guid productId, UpdateDepositProductRequest request, Guid updatedByUserId)
        => await _productService.UpdateDepositProductAsync(productId, request, updatedByUserId);

    public async Task<bool> DeactivateDepositProductAsync(Guid productId, Guid deactivatedByUserId)
        => await _productService.DeactivateDepositProductAsync(productId, deactivatedByUserId);

    #endregion

    #region Interest Tier Management (Delegates to IInterestTierService)

    public async Task<InterestTierDto> CreateInterestTierAsync(Guid productId, CreateInterestTierRequest request, Guid createdByUserId)
        => await _tierService.CreateInterestTierAsync(productId, request, createdByUserId);

    public async Task<InterestTierDto> UpdateInterestTierAsync(Guid tierId, UpdateInterestTierRequest request, Guid updatedByUserId)
        => await _tierService.UpdateInterestTierAsync(tierId, request, updatedByUserId);

    public async Task<bool> DeleteInterestTierAsync(Guid tierId, Guid deletedByUserId)
        => await _tierService.DeleteInterestTierAsync(tierId, deletedByUserId);

    public async Task<IEnumerable<InterestTierDto>> GetInterestTiersAsync(Guid productId)
        => await _tierService.GetInterestTiersAsync(productId);

    #endregion

    #region Fixed Deposit Management (Delegates to IFixedDepositService)

    public async Task<FixedDepositDto> CreateFixedDepositAsync(CreateFixedDepositRequest request, Guid customerId)
        => await _fixedDepositService.CreateFixedDepositAsync(request, customerId);

    public async Task<FixedDepositDto?> GetFixedDepositAsync(Guid depositId)
        => await _fixedDepositService.GetFixedDepositAsync(depositId);

    public async Task<FixedDepositDto?> GetFixedDepositByNumberAsync(string depositNumber)
        => await _fixedDepositService.GetFixedDepositByNumberAsync(depositNumber);

    public async Task<IEnumerable<FixedDepositDto>> GetCustomerFixedDepositsAsync(Guid customerId)
        => await _fixedDepositService.GetCustomerFixedDepositsAsync(customerId);

    public async Task<IEnumerable<FixedDepositDto>> GetMaturingDepositsAsync(DateTime fromDate, DateTime toDate)
        => await _fixedDepositService.GetMaturingDepositsAsync(fromDate, toDate);

    #endregion

    #region Interest Calculation and Processing (Delegates to appropriate services)

    public async Task<decimal> CalculateInterestAsync(Guid depositId, DateTime fromDate, DateTime toDate)
    {
        var deposit = await _unitOfWork.Repository<Bank.Domain.Entities.FixedDeposit>().GetByIdAsync(depositId);
        if (deposit == null)
            throw new InvalidOperationException($"Fixed deposit {depositId} not found");

        var principal = deposit.PrincipalAmount;
        var rate = deposit.InterestRate / 100;
        var days = (toDate - fromDate).Days;

        return deposit.InterestCalculationMethod switch
        {
            Bank.Domain.Enums.InterestCalculationMethod.Simple => CalculationHelper.CalculateSimpleInterest(principal, rate, days),
            Bank.Domain.Enums.InterestCalculationMethod.CompoundDaily => CalculationHelper.CalculateCompoundInterest(principal, rate, days, 365),
            Bank.Domain.Enums.InterestCalculationMethod.CompoundMonthly => CalculationHelper.CalculateCompoundInterest(principal, rate, days, 12),
            _ => CalculationHelper.CalculateSimpleInterest(principal, rate, days)
        };
    }

    public async Task<bool> ProcessInterestCreditAsync(Guid depositId, Guid processedByUserId)
    {
        var deposit = await _unitOfWork.Repository<Bank.Domain.Entities.FixedDeposit>().GetByIdAsync(depositId);
        if (deposit == null || deposit.Status != Bank.Domain.Enums.FixedDepositStatus.Active)
            return false;

        var fromDate = deposit.LastInterestCalculationDate;
        var toDate = DateTime.UtcNow;

        if ((toDate - fromDate).Days < 1)
            return false;

        var interestAmount = await CalculateInterestAsync(depositId, fromDate, toDate);
        if (interestAmount <= 0)
            return false;

        deposit.AccruedInterest += interestAmount;
        deposit.LastInterestCalculationDate = toDate;

        var transaction = new Bank.Domain.Entities.DepositTransaction
        {
            FixedDepositId = depositId,
            TransactionType = Bank.Domain.Enums.DepositTransactionType.InterestCredit,
            Amount = interestAmount,
            Description = $"Interest credit for period {fromDate:yyyy-MM-dd} to {toDate:yyyy-MM-dd}",
            TransactionDate = toDate,
            Status = Bank.Domain.Enums.TransactionStatus.Completed,
            InterestPeriodStart = fromDate,
            InterestPeriodEnd = toDate,
            InterestRate = deposit.InterestRate,
            InterestDays = (toDate - fromDate).Days,
            ProcessedByUserId = processedByUserId,
            ProcessedDate = DateTime.UtcNow,
            TransactionReference = GeneratorHelper.GenerateTransactionReference()
        };

        _unitOfWork.Repository<Bank.Domain.Entities.FixedDeposit>().Update(deposit);
        await _unitOfWork.Repository<Bank.Domain.Entities.DepositTransaction>().AddAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogSystemEventAsync(
            "DepositInterest",
            "Credit",
            depositId.ToString(),
            $"Credited interest {interestAmount:C} to deposit {deposit.DepositNumber}");

        return true;
    }

    public async Task<bool> ProcessDailyInterestAsync()
    {
        var activeDeposits = await _unitOfWork.Repository<Bank.Domain.Entities.FixedDeposit>()
            .FindAsync(d => d.Status == Bank.Domain.Enums.FixedDepositStatus.Active);

        var processedCount = 0;
        foreach (var deposit in activeDeposits)
        {
            try
            {
                if (await ProcessInterestCreditAsync(deposit.Id, Guid.Empty))
                    processedCount++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing daily interest for deposit {DepositId}", deposit.Id);
            }
        }

        _logger.LogInformation("Processed daily interest for {Count} deposits", processedCount);
        return true;
    }

    public async Task<bool> ProcessMonthlyInterestAsync()
        => await ProcessDailyInterestAsync();

    #endregion

    #region Maturity and Renewal Management

    public async Task<MaturityDetailsDto> GetMaturityDetailsAsync(Guid depositId)
    {
        var deposit = await _unitOfWork.Repository<Bank.Domain.Entities.FixedDeposit>().GetByIdAsync(depositId);

        if (deposit == null)
            throw new InvalidOperationException($"Fixed deposit {depositId} not found");

        var maturityAmount = deposit.CalculateMaturityAmount();
        var interestAtMaturity = deposit.CalculateInterestAtMaturity();

        var availableActions = new List<MaturityActionOption>
        {
            new() { Action = MaturityAction.TransferToPrimary, Description = "Transfer to primary account", RequiresCustomerConsent = false },
            new() { Action = MaturityAction.HoldForInstructions, Description = "Hold pending instructions", RequiresCustomerConsent = true }
        };

        if (deposit.DepositProduct?.AllowAutoRenewal == true)
        {
            availableActions.Add(new MaturityActionOption
            {
                Action = MaturityAction.AutoRenew,
                Description = "Auto-renew for same term",
                RequiresCustomerConsent = true
            });
        }

        return new MaturityDetailsDto
        {
            DepositId = depositId,
            MaturityDate = deposit.MaturityDate,
            PrincipalAmount = deposit.PrincipalAmount,
            AccruedInterest = deposit.AccruedInterest,
            MaturityAmount = maturityAmount,
            DefaultAction = deposit.MaturityAction,
            AutoRenewalEnabled = deposit.AutoRenewalEnabled,
            RenewalTermDays = deposit.RenewalTermDays,
            CustomerConsentReceived = deposit.CustomerConsentReceived,
            AvailableActions = availableActions
        };
    }

    public async Task<bool> ProcessMaturityAsync(Guid depositId, MaturityAction action, Guid processedByUserId)
    {
        var deposit = await _unitOfWork.Repository<Bank.Domain.Entities.FixedDeposit>().GetByIdAsync(depositId);

        if (deposit == null || deposit.Status != Bank.Domain.Enums.FixedDepositStatus.Active)
            return false;

        var maturityAmount = deposit.CalculateMaturityAmount();

        switch (action)
        {
            case MaturityAction.TransferToPrimary:
                return await ProcessMaturityTransferAsync(deposit, maturityAmount, processedByUserId);

            case MaturityAction.AutoRenew:
                return await ProcessAutoRenewalAsync(deposit, processedByUserId);

            case MaturityAction.HoldForInstructions:
                return await ProcessMaturityHoldAsync(deposit, processedByUserId);

            default:
                return false;
        }
    }

    private async Task<bool> ProcessMaturityTransferAsync(Bank.Domain.Entities.FixedDeposit deposit, decimal maturityAmount, Guid processedByUserId)
    {
        deposit.Status = Bank.Domain.Enums.FixedDepositStatus.Matured;
        deposit.ClosureDate = DateTime.UtcNow;
        deposit.NetAmountPaid = maturityAmount;

        var linkedAccount = await _unitOfWork.Repository<Bank.Domain.Entities.Account>().GetByIdAsync(deposit.LinkedAccountId);
        if (linkedAccount != null)
        {
            linkedAccount.Balance += maturityAmount;
        }

        var transaction = new Bank.Domain.Entities.DepositTransaction
        {
            FixedDepositId = deposit.Id,
            TransactionType = Bank.Domain.Enums.DepositTransactionType.MaturityPayout,
            Amount = maturityAmount,
            Description = $"Maturity payout for deposit {deposit.DepositNumber}",
            TransactionDate = DateTime.UtcNow,
            Status = Bank.Domain.Enums.TransactionStatus.Completed,
            ProcessedByUserId = processedByUserId,
            ProcessedDate = DateTime.UtcNow,
            TransactionReference = GeneratorHelper.GenerateTransactionReference()
        };

        _unitOfWork.Repository<Bank.Domain.Entities.FixedDeposit>().Update(deposit);
        if (linkedAccount != null)
            _unitOfWork.Repository<Bank.Domain.Entities.Account>().Update(linkedAccount);
        await _unitOfWork.Repository<Bank.Domain.Entities.DepositTransaction>().AddAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogUserActionAsync(
            processedByUserId,
            "FixedDeposit",
            "Maturity",
            deposit.Id.ToString(),
            $"Processed maturity for deposit {deposit.DepositNumber}, paid {maturityAmount:C}");

        return true;
    }

    private async Task<bool> ProcessAutoRenewalAsync(Bank.Domain.Entities.FixedDeposit deposit, Guid processedByUserId)
    {
        if (!deposit.AutoRenewalEnabled || !deposit.CustomerConsentReceived)
            return false;

        var renewalRequest = new RenewDepositRequest
        {
            TermDays = deposit.RenewalTermDays ?? deposit.TermDays,
            InterestRate = deposit.InterestRate,
            MaturityAction = deposit.MaturityAction,
            AutoRenewalEnabled = deposit.AutoRenewalEnabled
        };

        await RenewFixedDepositAsync(deposit.Id, renewalRequest, processedByUserId);
        return true;
    }

    private async Task<bool> ProcessMaturityHoldAsync(Bank.Domain.Entities.FixedDeposit deposit, Guid processedByUserId)
    {
        deposit.Status = Bank.Domain.Enums.FixedDepositStatus.PendingRenewal;
        _unitOfWork.Repository<Bank.Domain.Entities.FixedDeposit>().Update(deposit);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogUserActionAsync(
            processedByUserId,
            "FixedDeposit",
            "Hold",
            deposit.Id.ToString(),
            $"Deposit {deposit.DepositNumber} held pending customer instructions");

        return true;
    }

    public async Task<FixedDepositDto> RenewFixedDepositAsync(Guid depositId, RenewDepositRequest request, Guid processedByUserId)
    {
        var originalDeposit = await _unitOfWork.Repository<Bank.Domain.Entities.FixedDeposit>().GetByIdAsync(depositId);

        if (originalDeposit == null)
            throw new InvalidOperationException($"Fixed deposit {depositId} not found");

        var maturityAmount = originalDeposit.CalculateMaturityAmount();
        var termDays = request.TermDays ?? originalDeposit.TermDays;
        var interestRate = request.InterestRate ?? originalDeposit.DepositProduct?.GetApplicableRate(maturityAmount, termDays) ?? originalDeposit.InterestRate;

        // Close original deposit
        originalDeposit.Status = Bank.Domain.Enums.FixedDepositStatus.Renewed;
        originalDeposit.ClosureDate = DateTime.UtcNow;

        // Create new deposit
        var renewedDeposit = new Bank.Domain.Entities.FixedDeposit
        {
            CustomerId = originalDeposit.CustomerId,
            DepositProductId = originalDeposit.DepositProductId,
            LinkedAccountId = originalDeposit.LinkedAccountId,
            PrincipalAmount = maturityAmount,
            InterestRate = interestRate,
            TermDays = termDays,
            StartDate = DateTime.UtcNow,
            MaturityDate = DateTime.UtcNow.AddDays(termDays),
            Status = Bank.Domain.Enums.FixedDepositStatus.Active,
            InterestCalculationMethod = originalDeposit.InterestCalculationMethod,
            CompoundingFrequency = originalDeposit.CompoundingFrequency,
            LastInterestCalculationDate = DateTime.UtcNow,
            MaturityAction = request.MaturityAction ?? originalDeposit.MaturityAction,
            AutoRenewalEnabled = request.AutoRenewalEnabled ?? originalDeposit.AutoRenewalEnabled,
            PenaltyType = originalDeposit.PenaltyType,
            PenaltyAmount = originalDeposit.PenaltyAmount,
            PenaltyPercentage = originalDeposit.PenaltyPercentage,
            RenewedFromDepositId = originalDeposit.Id,
            RenewalCount = originalDeposit.RenewalCount + 1,
            DepositNumber = GeneratorHelper.GenerateDepositNumber()
        };

        originalDeposit.RenewedToDepositId = renewedDeposit.Id;

        _unitOfWork.Repository<Bank.Domain.Entities.FixedDeposit>().Update(originalDeposit);
        await _unitOfWork.Repository<Bank.Domain.Entities.FixedDeposit>().AddAsync(renewedDeposit);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogUserActionAsync(
            processedByUserId,
            "FixedDeposit",
            "Renew",
            renewedDeposit.Id.ToString(),
            $"Renewed deposit {originalDeposit.DepositNumber} to {renewedDeposit.DepositNumber}");

        return await _fixedDepositService.GetFixedDepositAsync(renewedDeposit.Id) ?? throw new InvalidOperationException("Failed to retrieve renewed deposit");
    }

    public async Task<bool> ProcessAutoRenewalsAsync()
    {
        var maturingDeposits = await _unitOfWork.Repository<Bank.Domain.Entities.FixedDeposit>()
            .FindAsync(d => d.Status == Bank.Domain.Enums.FixedDepositStatus.Active &&
                           d.MaturityDate <= DateTime.UtcNow &&
                           d.AutoRenewalEnabled &&
                           d.CustomerConsentReceived);

        var processedCount = 0;
        foreach (var deposit in maturingDeposits)
        {
            try
            {
                if (await ProcessAutoRenewalAsync(deposit, Guid.Empty))
                    processedCount++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing auto-renewal for deposit {DepositId}", deposit.Id);
            }
        }

        _logger.LogInformation("Processed auto-renewals for {Count} deposits", processedCount);
        return true;
    }

    #endregion

    #region Withdrawal Management (Delegates to IDepositWithdrawalService)

    public async Task<WithdrawalDetailsDto> CalculateEarlyWithdrawalAsync(Guid depositId, decimal withdrawalAmount)
    {
        var calculation = await _withdrawalService.CalculateDetailedWithdrawalAsync(depositId, withdrawalAmount);

        return new WithdrawalDetailsDto
        {
            DepositId = depositId,
            RequestedAmount = withdrawalAmount,
            AvailableBalance = calculation.AvailableBalance,
            PenaltyAmount = calculation.PenaltyAmount,
            NetAmount = calculation.NetAmount,
            PenaltyType = calculation.PenaltyType,
            PenaltyDescription = calculation.PenaltyDescription,
            IsEarlyWithdrawal = calculation.IsEarlyWithdrawal,
            DaysBeforeMaturity = calculation.DaysBeforeMaturity
        };
    }

    public async Task<bool> ProcessEarlyWithdrawalAsync(Guid depositId, EarlyWithdrawalRequest request, Guid processedByUserId)
    {
        var result = await _withdrawalService.ProcessEarlyWithdrawalWithDetailsAsync(depositId, request, processedByUserId);
        return result.Success;
    }

    public async Task<bool> ProcessPartialWithdrawalAsync(Guid depositId, PartialWithdrawalRequest request, Guid processedByUserId)
    {
        var deposit = await _unitOfWork.Repository<Bank.Domain.Entities.FixedDeposit>().GetByIdAsync(depositId);

        if (deposit == null || deposit.Status != Bank.Domain.Enums.FixedDepositStatus.Active)
            return false;

        if (!deposit.DepositProduct?.AllowPartialWithdrawals == true)
            throw new InvalidOperationException("Partial withdrawals not allowed for this deposit product");

        var availableBalance = deposit.PrincipalAmount + deposit.AccruedInterest;
        if (request.WithdrawalAmount > availableBalance)
            throw new InvalidOperationException("Withdrawal amount exceeds available balance");

        var remainingBalance = availableBalance - request.WithdrawalAmount;
        if (remainingBalance < deposit.DepositProduct?.MinimumBalance)
            throw new InvalidOperationException($"Remaining balance would be below minimum of {deposit.DepositProduct.MinimumBalance:C}");

        if (request.WithdrawalAmount <= deposit.PrincipalAmount)
        {
            deposit.PrincipalAmount -= request.WithdrawalAmount;
        }
        else
        {
            var interestWithdrawal = request.WithdrawalAmount - deposit.PrincipalAmount;
            deposit.PrincipalAmount = 0;
            deposit.AccruedInterest -= interestWithdrawal;
        }

        deposit.LinkedAccount.Balance += request.WithdrawalAmount;

        var transaction = new Bank.Domain.Entities.DepositTransaction
        {
            FixedDepositId = depositId,
            TransactionType = Bank.Domain.Enums.DepositTransactionType.PartialWithdrawal,
            Amount = request.WithdrawalAmount,
            Description = $"Partial withdrawal from deposit {deposit.DepositNumber}: {request.Reason}",
            TransactionDate = DateTime.UtcNow,
            Status = Bank.Domain.Enums.TransactionStatus.Completed,
            ProcessedByUserId = processedByUserId,
            ProcessedDate = DateTime.UtcNow,
            TransactionReference = GeneratorHelper.GenerateTransactionReference()
        };

        _unitOfWork.Repository<Bank.Domain.Entities.FixedDeposit>().Update(deposit);
        _unitOfWork.Repository<Bank.Domain.Entities.Account>().Update(deposit.LinkedAccount);
        await _unitOfWork.Repository<Bank.Domain.Entities.DepositTransaction>().AddAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogUserActionAsync(
            processedByUserId,
            "FixedDeposit",
            "PartialWithdrawal",
            depositId.ToString(),
            $"Processed partial withdrawal of {request.WithdrawalAmount:C} from deposit {deposit.DepositNumber}");

        return true;
    }

    #endregion

    #region Notice Management

    public async Task<MaturityNoticeDto> GenerateMaturityNoticeAsync(Guid depositId, MaturityNoticeType noticeType, Guid generatedByUserId)
    {
        var deposit = await _unitOfWork.Repository<Bank.Domain.Entities.FixedDeposit>().GetByIdAsync(depositId);

        if (deposit == null)
            throw new InvalidOperationException($"Fixed deposit {depositId} not found");

        var notice = new Bank.Domain.Entities.MaturityNotice
        {
            FixedDepositId = depositId,
            NoticeType = noticeType,
            NoticeDate = DateTime.UtcNow,
            MaturityDate = deposit.MaturityDate,
            Status = Bank.Domain.Enums.NotificationStatus.Pending,
            Subject = GenerateNoticeSubject(noticeType, deposit),
            Content = GenerateNoticeContent(noticeType, deposit),
            DeliveryChannel = Bank.Domain.Enums.NotificationChannel.Email,
            DeliveryAddress = deposit.Customer?.Email ?? string.Empty,
            GeneratedByUserId = generatedByUserId,
            NoticeNumber = GeneratorHelper.GenerateNoticeNumber(noticeType)
        };

        await _unitOfWork.Repository<Bank.Domain.Entities.MaturityNotice>().AddAsync(notice);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogUserActionAsync(
            generatedByUserId,
            "MaturityNotice",
            "Generate",
            notice.Id.ToString(),
            $"Generated {noticeType} notice {notice.NoticeNumber} for deposit {deposit.DepositNumber}");

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

    private static string GenerateNoticeSubject(MaturityNoticeType noticeType, Bank.Domain.Entities.FixedDeposit deposit)
    {
        return noticeType switch
        {
            MaturityNoticeType.Initial => $"Fixed Deposit Maturity Notice - {deposit.DepositNumber}",
            MaturityNoticeType.Reminder => $"Reminder: Fixed Deposit Maturing Soon - {deposit.DepositNumber}",
            MaturityNoticeType.Final => $"Final Notice: Fixed Deposit Maturity - {deposit.DepositNumber}",
            MaturityNoticeType.AutoRenewal => $"Auto-Renewal Confirmation - {deposit.DepositNumber}",
            _ => $"Fixed Deposit Notice - {deposit.DepositNumber}"
        };
    }

    private static string GenerateNoticeContent(MaturityNoticeType noticeType, Bank.Domain.Entities.FixedDeposit deposit)
    {
        var maturityAmount = deposit.CalculateMaturityAmount();
        var daysToMaturity = (deposit.MaturityDate - DateTime.UtcNow).Days;

        return $@"
Dear Customer,

Your fixed deposit {deposit.DepositNumber} will mature on {deposit.MaturityDate:yyyy-MM-dd} ({daysToMaturity} days from now).

Deposit Details:
- Principal Amount: {deposit.PrincipalAmount:C}
- Interest Rate: {deposit.InterestRate}%
- Maturity Amount: {maturityAmount:C}

Please contact us to provide instructions for the maturity proceeds.

Best regards,
Bank Customer Service
";
    }

    public async Task<bool> SendMaturityNoticesAsync()
    {
        var maturingDeposits = await _unitOfWork.Repository<Bank.Domain.Entities.FixedDeposit>()
            .FindAsync(d => d.Status == Bank.Domain.Enums.FixedDepositStatus.Active &&
                           d.MaturityDate <= DateTime.UtcNow.AddDays(30) &&
                           d.MaturityDate > DateTime.UtcNow);

        var processedCount = 0;
        foreach (var deposit in maturingDeposits)
        {
            try
            {
                var existingNotices = await _unitOfWork.Repository<Bank.Domain.Entities.MaturityNotice>()
                    .FindAsync(n => n.FixedDepositId == deposit.Id);

                if (!existingNotices.Any())
                {
                    await GenerateMaturityNoticeAsync(deposit.Id, MaturityNoticeType.Initial, Guid.Empty);
                    processedCount++;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending maturity notice for deposit {DepositId}", deposit.Id);
            }
        }

        _logger.LogInformation("Sent maturity notices for {Count} deposits", processedCount);
        return true;
    }

    public async Task<bool> ProcessCustomerResponseAsync(Guid noticeId, MaturityAction customerChoice, string? instructions, Guid processedByUserId)
    {
        var notice = await _unitOfWork.Repository<Bank.Domain.Entities.MaturityNotice>().GetByIdAsync(noticeId);
        if (notice == null)
            return false;

        notice.RecordCustomerResponse(customerChoice, instructions);

        var deposit = await _unitOfWork.Repository<Bank.Domain.Entities.FixedDeposit>().GetByIdAsync(notice.FixedDepositId);
        if (deposit != null)
        {
            deposit.MaturityAction = customerChoice;
            deposit.CustomerConsentReceived = true;
            _unitOfWork.Repository<Bank.Domain.Entities.FixedDeposit>().Update(deposit);
        }

        _unitOfWork.Repository<Bank.Domain.Entities.MaturityNotice>().Update(notice);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogUserActionAsync(
            processedByUserId,
            "MaturityNotice",
            "CustomerResponse",
            noticeId.ToString(),
            $"Recorded customer response: {customerChoice} for notice {notice.NoticeNumber}");

        return true;
    }

    #endregion

    #region Reporting and Analytics

    public async Task<DepositSummaryDto> GetDepositSummaryAsync(Guid customerId)
    {
        var deposits = await _unitOfWork.Repository<Bank.Domain.Entities.FixedDeposit>()
            .FindAsync(d => d.CustomerId == customerId);

        var activeDeposits = deposits.Where(d => d.Status == Bank.Domain.Enums.FixedDepositStatus.Active).ToList();
        var maturingThisMonth = activeDeposits.Where(d => d.MaturityDate <= DateTime.UtcNow.AddDays(30)).Count();

        return new DepositSummaryDto
        {
            CustomerId = customerId,
            TotalDeposits = deposits.Count(),
            TotalPrincipal = activeDeposits.Sum(d => d.PrincipalAmount),
            TotalAccruedInterest = activeDeposits.Sum(d => d.AccruedInterest),
            TotalMaturityValue = activeDeposits.Sum(d => d.CalculateMaturityAmount()),
            ActiveDeposits = activeDeposits.Count,
            MaturingThisMonth = maturingThisMonth,
            AverageInterestRate = activeDeposits.Any() ? activeDeposits.Average(d => d.InterestRate) : 0
        };
    }

    public async Task<IEnumerable<DepositTransactionDto>> GetDepositTransactionsAsync(Guid depositId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var transactions = await _unitOfWork.Repository<Bank.Domain.Entities.DepositTransaction>()
            .FindAsync(t => t.FixedDepositId == depositId &&
                           (!fromDate.HasValue || t.TransactionDate >= fromDate.Value) &&
                           (!toDate.HasValue || t.TransactionDate <= toDate.Value));

        return transactions.OrderByDescending(t => t.TransactionDate).Select(t => new DepositTransactionDto
        {
            Id = t.Id,
            FixedDepositId = t.FixedDepositId,
            TransactionType = t.TransactionType,
            Amount = t.Amount,
            Description = t.Description,
            TransactionDate = t.TransactionDate,
            Status = t.Status,
            TransactionReference = t.TransactionReference
        });
    }

    public async Task<DepositPortfolioDto> GetCustomerDepositPortfolioAsync(Guid customerId)
    {
        var summary = await GetDepositSummaryAsync(customerId);
        var activeDeposits = await GetCustomerFixedDepositsAsync(customerId);
        var maturingDeposits = activeDeposits.Where(d => d.Status == Bank.Domain.Enums.FixedDepositStatus.Active &&
                                                         d.MaturityDate <= DateTime.UtcNow.AddDays(30)).ToList();

        var recentTransactions = new List<DepositTransactionDto>();
        foreach (var deposit in activeDeposits.Take(5))
        {
            var transactions = await GetDepositTransactionsAsync(deposit.Id, DateTime.UtcNow.AddDays(-30));
            recentTransactions.AddRange(transactions.Take(10));
        }

        return new DepositPortfolioDto
        {
            CustomerId = customerId,
            CustomerName = string.Empty,
            Summary = summary,
            ActiveDeposits = activeDeposits.Where(d => d.Status == Bank.Domain.Enums.FixedDepositStatus.Active).ToList(),
            MaturingDeposits = maturingDeposits,
            RecentTransactions = recentTransactions.OrderByDescending(t => t.TransactionDate).Take(20).ToList()
        };
    }

    #endregion

    #region Background Processing

    public async Task<bool> ProcessMaturityNoticesAsync()
        => await SendMaturityNoticesAsync();

    public async Task<bool> ProcessPendingMaturityActionsAsync()
    {
        var maturingDeposits = await _unitOfWork.Repository<Bank.Domain.Entities.FixedDeposit>()
            .FindAsync(d => d.Status == Bank.Domain.Enums.FixedDepositStatus.Active &&
                           d.MaturityDate <= DateTime.UtcNow);

        var processedCount = 0;
        foreach (var deposit in maturingDeposits)
        {
            try
            {
                if (await ProcessMaturityAsync(deposit.Id, deposit.MaturityAction, Guid.Empty))
                    processedCount++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing maturity for deposit {DepositId}", deposit.Id);
            }
        }

        _logger.LogInformation("Processed maturity actions for {Count} deposits", processedCount);
        return true;
    }

    public async Task<bool> ProcessInterestAccrualsAsync()
        => await ProcessDailyInterestAsync();

    #endregion
}
