using Bank.Application.DTOs;
using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Application.Utilities;

/// <summary>
/// Centralized helper for DTO mapping operations
/// </summary>
public static class MappingHelper
{
    /// <summary>
    /// Maps Account entity to AccountDto
    /// </summary>
    public static AccountDto MapToAccountDto(Account account)
    {
        return new AccountDto
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            AccountHolderName = account.AccountHolderName,
            Balance = account.Balance,
            Status = account.Status,
            Type = account.Type,
            OpenedDate = account.OpenedDate,
            ClosedDate = account.ClosedDate,
            ClosureReason = account.ClosureReason,
            LastActivityDate = account.LastActivityDate,
            DormancyDate = account.DormancyDate,
            MinimumBalance = account.MinimumBalance,
            MonthlyMaintenanceFee = account.MonthlyMaintenanceFee,
            FeeWaiverEligible = account.FeeWaiverEligible,
            InterestRate = account.InterestRate,
            LastInterestCalculationDate = account.LastInterestCalculationDate,
            CompoundingFrequency = account.CompoundingFrequency,
            IsJointAccount = account.IsJointAccount,
            RequiresMultipleSignatures = account.RequiresMultipleSignatures,
            MultipleSignatureThreshold = account.MultipleSignatureThreshold,
            MinimumSignaturesRequired = account.MinimumSignaturesRequired,
            HasHolds = account.HasHolds,
            HasRestrictions = account.HasRestrictions
        };
    }

    /// <summary>
    /// Maps Transaction entity to TransactionDto
    /// </summary>
    public static TransactionDto MapToTransactionDto(Transaction transaction)
    {
        return new TransactionDto
        {
            Id = transaction.Id,
            AccountId = transaction.FromAccountId, // Use FromAccountId as primary account
            Amount = transaction.Amount,
            TransactionType = transaction.Type,
            Description = transaction.Description,
            Reference = transaction.Reference,
            Status = transaction.Status,
            CreatedAt = transaction.CreatedAt,
            ProcessedAt = transaction.ProcessedAt
        };
    }

    /// <summary>
    /// Maps User entity to ProfileDto
    /// </summary>
    public static ProfileDto MapToProfileDto(User user)
    {
        return new ProfileDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt ?? user.CreatedAt
        };
    }

    /// <summary>
    /// Maps Loan entity to LoanDto
    /// </summary>
    public static LoanDto MapToLoanDto(Loan loan)
    {
        return new LoanDto
        {
            Id = loan.Id,
            LoanNumber = loan.LoanNumber,
            Type = loan.Type,
            TypeName = loan.Type.ToString(),
            PrincipalAmount = loan.PrincipalAmount,
            InterestRate = loan.InterestRate,
            TermInMonths = loan.TermInMonths,
            Status = loan.Status,
            StatusName = loan.Status.ToString(),
            ApplicationDate = loan.ApplicationDate,
            ApprovalDate = loan.ApprovalDate,
            DisbursementDate = loan.DisbursementDate,
            MaturityDate = loan.MaturityDate,
            OutstandingBalance = loan.OutstandingBalance,
            MonthlyPaymentAmount = loan.MonthlyPaymentAmount,
            NextPaymentDueDate = loan.NextPaymentDueDate,
            DaysOverdue = loan.DaysOverdue,
            TotalInterestPaid = loan.TotalInterestPaid,
            TotalPrincipalPaid = loan.TotalPrincipalPaid,
            Purpose = loan.Purpose,
            CreditScore = loan.CreditScore,
            CreditScoreRange = loan.CreditScoreRange
        };
    }

    /// <summary>
    /// Maps Card entity to CardDto
    /// </summary>
    public static CardDto MapToCardDto(Card card)
    {
        return new CardDto
        {
            Id = card.Id,
            CardNumber = MaskCardNumber(card.CardNumber),
            CardType = card.Type,
            Status = card.Status,
            ExpiryDate = card.ExpiryDate,
            AccountId = card.AccountId,
            CustomerId = card.CustomerId,
            CreatedAt = card.CreatedAt,
            UpdatedAt = card.UpdatedAt ?? card.CreatedAt,
            IsActive = card.IsActive()
        };
    }

    /// <summary>
    /// Maps Beneficiary entity to BeneficiaryDto
    /// </summary>
    public static BeneficiaryDto MapToBeneficiaryDto(Beneficiary beneficiary)
    {
        return new BeneficiaryDto
        {
            Id = beneficiary.Id,
            CustomerId = beneficiary.CustomerId,
            Name = beneficiary.Name,
            AccountNumber = beneficiary.AccountNumber,
            BankName = beneficiary.BankName,
            RoutingNumber = beneficiary.RoutingNumber,
            Type = beneficiary.Type,
            IsActive = beneficiary.IsActive,
            CreatedAt = beneficiary.CreatedAt,
            UpdatedAt = beneficiary.UpdatedAt ?? beneficiary.CreatedAt
        };
    }

    /// <summary>
    /// Maps Notification entity to NotificationDto
    /// </summary>
    public static NotificationDto MapToNotificationDto(Notification notification)
    {
        return new NotificationDto
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Type = notification.Type,
            Title = notification.Subject,
            Message = notification.Message,
            IsRead = notification.ReadAt.HasValue,
            Priority = notification.Priority,
            CreatedAt = notification.CreatedAt,
            ReadAt = notification.ReadAt,
            ExpiresAt = notification.ExpiresAt
        };
    }

    /// <summary>
    /// Maps AuditLog entity to AuditLogDto
    /// </summary>
    public static AuditLogDto MapToAuditLogDto(AuditLog auditLog)
    {
        return new AuditLogDto
        {
            Id = auditLog.Id,
            UserId = auditLog.UserId,
            Action = auditLog.Action,
            EntityType = auditLog.EntityType,
            EntityId = auditLog.EntityId,
            OldValues = auditLog.OldValues,
            NewValues = auditLog.NewValues,
            Timestamp = auditLog.CreatedAt,
            IpAddress = auditLog.IpAddress,
            UserAgent = auditLog.UserAgent
        };
    }

    /// <summary>
    /// Maps RecurringPayment entity to RecurringPaymentDto
    /// </summary>
    public static RecurringPaymentDto MapToRecurringPaymentDto(RecurringPayment payment)
    {
        return new RecurringPaymentDto
        {
            Id = payment.Id,
            CustomerId = payment.CreatedByUserId, // Use CreatedByUserId as CustomerId
            BeneficiaryId = payment.ToAccountId, // Use ToAccountId as BeneficiaryId
            Amount = payment.Amount,
            Currency = "USD", // Default currency since not in entity
            Frequency = payment.Frequency.ToString(),
            StartDate = payment.StartDate,
            EndDate = payment.EndDate,
            NextExecutionDate = payment.NextExecutionDate,
            Status = payment.Status.ToString(),
            Description = payment.Description,
            CreatedAt = payment.CreatedAt,
            UpdatedAt = payment.UpdatedAt ?? payment.CreatedAt
        };
    }

    /// <summary>
    /// Maps Session entity to SessionDto
    /// </summary>
    public static SessionDto MapToSessionDto(Session session)
    {
        return new SessionDto
        {
            Id = session.Id,
            UserId = session.UserId,
            Token = session.SessionToken,
            IsActive = session.Status == SessionStatus.Active,
            CreatedAt = session.CreatedAt,
            ExpiresAt = session.ExpiresAt,
            LastAccessedAt = session.LastActivityAt,
            IpAddress = session.IpAddress,
            UserAgent = session.UserAgent
        };
    }

    /// <summary>
    /// Maps multiple entities to DTOs
    /// </summary>
    public static List<TDto> MapToList<TEntity, TDto>(IEnumerable<TEntity> entities, Func<TEntity, TDto> mapper)
    {
        return entities?.Select(mapper).ToList() ?? new List<TDto>();
    }

    /// <summary>
    /// Maps paged result to paged DTO result
    /// </summary>
    public static Bank.Application.DTOs.PagedResult<TDto> MapToPagedResult<TEntity, TDto>(
        Bank.Domain.Common.PagedResult<TEntity> pagedResult, Func<TEntity, TDto> mapper)
    {
        return new Bank.Application.DTOs.PagedResult<TDto>
        {
            Items = MapToList(pagedResult.Items, mapper),
            TotalCount = pagedResult.TotalCount,
            Page = pagedResult.Page,
            PageSize = pagedResult.PageSize
        };
    }

    /// <summary>
    /// Creates a standardized success response
    /// </summary>
    public static ApiResponse<T> CreateSuccessResponse<T>(T data, string message = "Operation completed successfully")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// Creates a standardized error response
    /// </summary>
    public static ApiResponse<T> CreateErrorResponse<T>(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }

    /// <summary>
    /// Creates a standardized validation error response
    /// </summary>
    public static ApiResponse<T> CreateValidationErrorResponse<T>(Dictionary<string, List<string>> validationErrors)
    {
        var allErrors = validationErrors.SelectMany(kvp => kvp.Value).ToList();
        return new ApiResponse<T>
        {
            Success = false,
            Message = "Validation failed",
            Errors = allErrors,
            ValidationErrors = validationErrors
        };
    }

    /// <summary>
    /// Maps DepositTransaction entity to DepositTransactionDto
    /// </summary>
    public static DepositTransactionDto MapToDepositTransactionDto(DepositTransaction transaction)
    {
        return new DepositTransactionDto
        {
            Id = transaction.Id,
            FixedDepositId = transaction.FixedDepositId,
            TransactionReference = transaction.TransactionReference,
            TransactionType = transaction.TransactionType,
            Amount = transaction.Amount,
            Description = transaction.Description,
            TransactionDate = transaction.TransactionDate,
            Status = transaction.Status,
            InterestPeriodStart = transaction.InterestPeriodStart,
            InterestPeriodEnd = transaction.InterestPeriodEnd,
            InterestRate = transaction.InterestRate,
            InterestDays = transaction.InterestDays,
            PenaltyType = transaction.PenaltyType,
            PenaltyReason = transaction.PenaltyReason
        };
    }

    #region Private Helper Methods

    private static string MaskCardNumber(string cardNumber)
    {
        if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 8)
            return cardNumber;

        // Show first 4 and last 4 digits, mask the middle
        var first4 = cardNumber[..4];
        var last4 = cardNumber[^4..];
        var masked = new string('*', cardNumber.Length - 8);
        
        return $"{first4}{masked}{last4}";
    }

    #endregion
}

/// <summary>
/// Generic API response class
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();
    public Dictionary<string, List<string>>? ValidationErrors { get; set; }
}