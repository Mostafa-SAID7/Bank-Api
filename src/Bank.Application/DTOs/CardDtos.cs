using Bank.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bank.Application.DTOs;

public class CardDto
{
    public Guid Id { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public CardType CardType { get; set; }
    public CardStatus Status { get; set; }
    public DateTime ExpiryDate { get; set; }
    public Guid AccountId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}

#region Request DTOs

/// <summary>
/// Request to issue a new card
/// </summary>
public class CardIssuanceRequest
{
    [Required]
    public Guid CustomerId { get; set; }
    
    [Required]
    public Guid AccountId { get; set; }
    
    [Required]
    public CardType CardType { get; set; }
    
    public string? CardName { get; set; }
    
    public decimal? DailyLimit { get; set; }
    
    public decimal? MonthlyLimit { get; set; }
    
    public decimal? AtmDailyLimit { get; set; }
    
    public bool ContactlessEnabled { get; set; } = true;
    
    public bool OnlineTransactionsEnabled { get; set; } = true;
    
    public bool InternationalTransactionsEnabled { get; set; } = false;
    
    public List<MerchantCategory>? BlockedMerchantCategories { get; set; }
}

/// <summary>
/// Request to activate a card
/// </summary>
public class CardActivationRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public Guid CustomerId { get; set; }
    
    [Required]
    public string ActivationCode { get; set; } = string.Empty;
    
    [Required]
    public CardActivationChannel Channel { get; set; }
    
    public string? Pin { get; set; }
}

/// <summary>
/// Request to block a card
/// </summary>
public class CardBlockRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public Guid CustomerId { get; set; }
    
    [Required]
    public CardBlockReason Reason { get; set; }
    
    public string? Notes { get; set; }
}

/// <summary>
/// Request to unblock a card
/// </summary>
public class CardUnblockRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public Guid CustomerId { get; set; }
    
    public string? Notes { get; set; }
}

/// <summary>
/// Request to update card limits
/// </summary>
public class CardLimitUpdateRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public Guid CustomerId { get; set; }
    
    [Range(0, 100000)]
    public decimal? DailyLimit { get; set; }
    
    [Range(0, 1000000)]
    public decimal? MonthlyLimit { get; set; }
    
    [Range(0, 50000)]
    public decimal? AtmDailyLimit { get; set; }
}

/// <summary>
/// Request to search card transactions
/// </summary>
public class CardTransactionSearchRequest : PagedRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public Guid CustomerId { get; set; }
    
    public DateTime? FromDate { get; set; }
    
    public DateTime? ToDate { get; set; }
    
    public CardTransactionType? TransactionType { get; set; }
    
    public CardTransactionStatus? Status { get; set; }
    
    public decimal? MinAmount { get; set; }
    
    public decimal? MaxAmount { get; set; }
    
    public string? MerchantName { get; set; }
    
    public MerchantCategory? MerchantCategory { get; set; }
    
    public bool? IsInternational { get; set; }
}

/// <summary>
/// Request to change card PIN
/// </summary>
public class CardPinChangeRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public Guid CustomerId { get; set; }
    
    [Required]
    [StringLength(4, MinimumLength = 4)]
    public string CurrentPin { get; set; } = string.Empty;
    
    [Required]
    [StringLength(4, MinimumLength = 4)]
    public string NewPin { get; set; } = string.Empty;
}

/// <summary>
/// Request to reset card PIN
/// </summary>
public class CardPinResetRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public Guid CustomerId { get; set; }
    
    [Required]
    public string VerificationCode { get; set; } = string.Empty;
}

/// <summary>
/// Request to update merchant restrictions
/// </summary>
public class CardMerchantRestrictionsRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public Guid CustomerId { get; set; }
    
    public List<MerchantCategory> BlockedCategories { get; set; } = new();
}

/// <summary>
/// Request to update contactless settings
/// </summary>
public class CardContactlessRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public Guid CustomerId { get; set; }
    
    [Required]
    public bool Enabled { get; set; }
}

/// <summary>
/// Request to update online transactions settings
/// </summary>
public class CardOnlineTransactionsRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public Guid CustomerId { get; set; }
    
    [Required]
    public bool Enabled { get; set; }
}

/// <summary>
/// Request to update international transactions settings
/// </summary>
public class CardInternationalTransactionsRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public Guid CustomerId { get; set; }
    
    [Required]
    public bool Enabled { get; set; }
}

/// <summary>
/// Request to validate card for transaction
/// </summary>
public class CardValidationRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    public CardTransactionType TransactionType { get; set; }
    
    public MerchantCategory? MerchantCategory { get; set; }
    
    public bool IsInternational { get; set; } = false;
    
    public bool IsOnline { get; set; } = false;
    
    public string? Pin { get; set; }
}

#endregion

#region Response DTOs

/// <summary>
/// Result of card issuance operation
/// </summary>
public class CardIssuanceResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? CardId { get; set; }
    public string? MaskedCardNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? ActivationCode { get; set; }
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Result of card activation operation
/// </summary>
public class CardActivationResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime? ActivationDate { get; set; }
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Result of card block/unblock operation
/// </summary>
public class CardBlockResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public CardStatus? NewStatus { get; set; }
    public DateTime? StatusChangeDate { get; set; }
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Result of card limit update operation
/// </summary>
public class CardLimitUpdateResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public decimal? NewDailyLimit { get; set; }
    public decimal? NewMonthlyLimit { get; set; }
    public decimal? NewAtmDailyLimit { get; set; }
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Result of PIN change operation
/// </summary>
public class CardPinChangeResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime? PinChangeDate { get; set; }
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Result of PIN reset operation
/// </summary>
public class CardPinResetResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? NewPin { get; set; }
    public DateTime? PinResetDate { get; set; }
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Result of merchant restrictions update
/// </summary>
public class CardMerchantRestrictionsResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<MerchantCategory> BlockedCategories { get; set; } = new();
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Result of contactless settings update
/// </summary>
public class CardContactlessResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool ContactlessEnabled { get; set; }
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Result of online transactions settings update
/// </summary>
public class CardOnlineTransactionsResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool OnlineTransactionsEnabled { get; set; }
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Result of international transactions settings update
/// </summary>
public class CardInternationalTransactionsResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool InternationalTransactionsEnabled { get; set; }
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Result of card validation
/// </summary>
public class CardValidationResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; } = string.Empty;
    public string ResponseCode { get; set; } = string.Empty;
    public string ResponseMessage { get; set; } = string.Empty;
    public string? DeclineReason { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
    public decimal? AvailableBalance { get; set; }
    public decimal? DailyLimitRemaining { get; set; }
    public decimal? MonthlyLimitRemaining { get; set; }
}

#endregion

#region Data DTOs

/// <summary>
/// Detailed card information
/// </summary>
public class CardDetailsDto
{
    public Guid Id { get; set; }
    public string MaskedCardNumber { get; set; } = string.Empty;
    public CardType Type { get; set; }
    public CardStatus Status { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime? ActivationDate { get; set; }
    public CardActivationChannel? ActivationChannel { get; set; }
    public decimal DailyLimit { get; set; }
    public decimal MonthlyLimit { get; set; }
    public decimal AtmDailyLimit { get; set; }
    public bool ContactlessEnabled { get; set; }
    public bool OnlineTransactionsEnabled { get; set; }
    public bool InternationalTransactionsEnabled { get; set; }
    public string? CardName { get; set; }
    public List<MerchantCategory> BlockedMerchantCategories { get; set; } = new();
    public DateTime? LastBlockedDate { get; set; }
    public CardBlockReason? LastBlockReason { get; set; }
    public bool HasPin { get; set; }
    public DateTime? PinSetDate { get; set; }
    public AccountSummaryDto Account { get; set; } = null!;
}

/// <summary>
/// Summary card information for listing
/// </summary>
public class CardSummaryDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid AccountId { get; set; }
    public string MaskedCardNumber { get; set; } = string.Empty;
    public CardType Type { get; set; }
    public CardStatus Status { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string? CardName { get; set; }
    public bool IsActive { get; set; }
    public bool IsExpired { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public decimal DailyLimit { get; set; }
    public decimal MonthlyLimit { get; set; }
    public bool IsContactlessEnabled { get; set; }
    public bool IsOnlineTransactionsEnabled { get; set; }
    public bool IsInternationalTransactionsEnabled { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Card transaction information
/// </summary>
public class CardTransactionDto
{
    public Guid Id { get; set; }
    public Guid CardId { get; set; }
    public string NetworkTransactionId { get; set; } = string.Empty;
    public string? AuthorizationCode { get; set; }
    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public decimal? OriginalAmount { get; set; }
    public string? OriginalCurrencyCode { get; set; }
    public CardTransactionType Type { get; set; }
    public CardTransactionType TransactionType { get; set; }
    public CardTransactionStatus Status { get; set; }
    public DateTime TransactionDate { get; set; }
    public DateTime? SettlementDate { get; set; }
    public string? MerchantName { get; set; }
    public MerchantCategory? MerchantCategory { get; set; }
    public string? MerchantCity { get; set; }
    public string? MerchantCountryCode { get; set; }
    public bool IsContactless { get; set; }
    public bool IsOnline { get; set; }
    public bool IsInternational { get; set; }
    public string? Description { get; set; }
    public string? Reference { get; set; }
    public decimal Fee { get; set; }
    public decimal? BalanceAfterTransaction { get; set; }
}

/// <summary>
/// Card usage statistics
/// </summary>
public class CardUsageStatsDto
{
    public Guid CardId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int TotalTransactions { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalFees { get; set; }
    public int PurchaseCount { get; set; }
    public decimal PurchaseAmount { get; set; }
    public int WithdrawalCount { get; set; }
    public decimal WithdrawalAmount { get; set; }
    public int OnlineTransactionCount { get; set; }
    public decimal OnlineTransactionAmount { get; set; }
    public int InternationalTransactionCount { get; set; }
    public decimal InternationalTransactionAmount { get; set; }
    public decimal DailyLimitUtilization { get; set; }
    public decimal MonthlyLimitUtilization { get; set; }
    public List<MerchantCategoryUsageDto> MerchantCategoryBreakdown { get; set; } = new();
}

/// <summary>
/// Merchant category usage breakdown
/// </summary>
public class MerchantCategoryUsageDto
{
    public MerchantCategory Category { get; set; }
    public int TransactionCount { get; set; }
    public decimal Amount { get; set; }
    public decimal Percentage { get; set; }
}

#endregion

#region Base DTOs

/// <summary>
/// Base class for paged requests
/// </summary>
public class PagedRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = true;
}

/// <summary>
/// Paged result wrapper - using Domain PagedResult
/// </summary>
public class PagedResult<T> : Bank.Domain.Common.PagedResult<T>
{
    // Inherits from Domain PagedResult to avoid duplication
}

/// <summary>
/// Account summary for card details
/// </summary>
public class AccountSummaryDto
{
    public Guid Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountHolderName { get; set; } = string.Empty;
    public AccountType Type { get; set; }
    public decimal Balance { get; set; }
}

#endregion
#region Card Network Integration DTOs

/// <summary>
/// Request for card transaction authorization
/// </summary>
public class CardAuthorizationRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    public string Currency { get; set; } = "USD";
    
    [Required]
    public string MerchantId { get; set; } = string.Empty;
    
    [Required]
    public string MerchantName { get; set; } = string.Empty;
    
    public MerchantCategory MerchantCategory { get; set; }
    
    public string? MerchantCountry { get; set; }
    
    public string? TransactionReference { get; set; }
    
    public bool IsInternational { get; set; }
    
    public bool IsOnline { get; set; }
    
    public bool IsContactless { get; set; }
    
    public string? AuthorizationCode { get; set; }
    
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Result of card authorization
/// </summary>
public class CardAuthorizationResult
{
    public bool Success { get; set; }
    public string AuthorizationCode { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public string ResponseCode { get; set; } = string.Empty;
    public string ResponseMessage { get; set; } = string.Empty;
    public decimal AuthorizedAmount { get; set; }
    public DateTime AuthorizationDate { get; set; }
    public string? DeclineReason { get; set; }
    public CardTransactionFees? Fees { get; set; }
}

/// <summary>
/// Request for capturing authorized transaction
/// </summary>
public class CardCaptureRequest
{
    [Required]
    public string AuthorizationCode { get; set; } = string.Empty;
    
    [Required]
    public decimal CaptureAmount { get; set; }
    
    public string? Reference { get; set; }
}

/// <summary>
/// Result of transaction capture
/// </summary>
public class CardCaptureResult
{
    public bool Success { get; set; }
    public string CaptureId { get; set; } = string.Empty;
    public decimal CapturedAmount { get; set; }
    public DateTime CaptureDate { get; set; }
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Request for voiding transaction
/// </summary>
public class CardVoidRequest
{
    [Required]
    public string AuthorizationCode { get; set; } = string.Empty;
    
    public string? Reason { get; set; }
}

/// <summary>
/// Result of transaction void
/// </summary>
public class CardVoidResult
{
    public bool Success { get; set; }
    public string VoidId { get; set; } = string.Empty;
    public DateTime VoidDate { get; set; }
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Request for transaction refund
/// </summary>
public class CardRefundRequest
{
    [Required]
    public string TransactionId { get; set; } = string.Empty;
    
    [Required]
    public decimal RefundAmount { get; set; }
    
    public string? Reason { get; set; }
}

/// <summary>
/// Result of transaction refund
/// </summary>
public class CardRefundResult
{
    public bool Success { get; set; }
    public string RefundId { get; set; } = string.Empty;
    public decimal RefundedAmount { get; set; }
    public DateTime RefundDate { get; set; }
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Request for settlement processing
/// </summary>
public class CardSettlementRequest
{
    public DateTime SettlementDate { get; set; } = DateTime.UtcNow.Date;
    public List<string>? TransactionIds { get; set; }
    public CardNetwork? Network { get; set; }
}

/// <summary>
/// Result of settlement processing
/// </summary>
public class CardSettlementResult
{
    public bool Success { get; set; }
    public string SettlementId { get; set; } = string.Empty;
    public DateTime SettlementDate { get; set; }
    public int TransactionCount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalFees { get; set; }
    public decimal NetAmount { get; set; }
    public List<string> ProcessedTransactions { get; set; } = new();
    public List<string> FailedTransactions { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Settlement report
/// </summary>
public class CardSettlementReport
{
    public DateTime SettlementDate { get; set; }
    public List<CardSettlementSummary> NetworkSummaries { get; set; } = new();
    public decimal TotalSettlementAmount { get; set; }
    public decimal TotalFees { get; set; }
    public decimal NetSettlementAmount { get; set; }
    public int TotalTransactionCount { get; set; }
}

/// <summary>
/// Settlement summary by network
/// </summary>
public class CardSettlementSummary
{
    public CardNetwork Network { get; set; }
    public int TransactionCount { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal InterchangeFees { get; set; }
    public decimal ProcessingFees { get; set; }
    public decimal NetAmount { get; set; }
}

/// <summary>
/// Request for card transaction processing
/// </summary>
public class CardTransactionRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    public CardTransactionType TransactionType { get; set; }
    
    [Required]
    public string MerchantId { get; set; } = string.Empty;
    
    [Required]
    public string MerchantName { get; set; } = string.Empty;
    
    public MerchantCategory MerchantCategory { get; set; }
    
    public string Currency { get; set; } = "USD";
    
    public string? Description { get; set; }
    
    public string? Reference { get; set; }
    
    public bool IsInternational { get; set; }
    
    public bool IsOnline { get; set; }
    
    public bool IsContactless { get; set; }
    
    public string? AuthorizationCode { get; set; }
    
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Result of card transaction processing
/// </summary>
public class CardTransactionResult
{
    public bool Success { get; set; }
    public Guid TransactionId { get; set; }
    public string AuthorizationCode { get; set; } = string.Empty;
    public CardTransactionStatus Status { get; set; }
    public decimal ProcessedAmount { get; set; }
    public decimal AccountBalance { get; set; }
    public CardTransactionFees? Fees { get; set; }
    public string? ErrorMessage { get; set; }
    public string? DeclineReason { get; set; }
}

/// <summary>
/// Request for card statement generation
/// </summary>
public class CardStatementRequest
{
    [Required]
    public Guid CardId { get; set; }
    
    [Required]
    public DateTime FromDate { get; set; }
    
    [Required]
    public DateTime ToDate { get; set; }
    
    public StatementFormat Format { get; set; } = StatementFormat.PDF;
    
    public bool IncludeTransactionDetails { get; set; } = true;
    
    public bool IncludeFeeBreakdown { get; set; } = true;
    
    public string? DeliveryEmail { get; set; }
}

/// <summary>
/// Result of card statement generation
/// </summary>
public class CardStatementResult
{
    public bool Success { get; set; }
    public Guid StatementId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public byte[]? Content { get; set; }
    public StatementFormat Format { get; set; }
    public DateTime GeneratedDate { get; set; }
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Card statement DTO
/// </summary>
public class CardStatementDto
{
    public Guid Id { get; set; }
    public Guid CardId { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public DateTime GeneratedDate { get; set; }
    public StatementFormat Format { get; set; }
    public string FileName { get; set; } = string.Empty;
    public int TransactionCount { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal TotalFees { get; set; }
    public decimal PreviousBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal MinimumPayment { get; set; }
    public DateTime? PaymentDueDate { get; set; }
}

/// <summary>
/// Request for card renewal
/// </summary>
public class CardRenewalResult
{
    public bool Success { get; set; }
    public int ProcessedCount { get; set; }
    public int SuccessfulRenewals { get; set; }
    public int FailedRenewals { get; set; }
    public List<CardRenewalInfo> RenewalDetails { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Card renewal information
/// </summary>
public class CardRenewalInfo
{
    public Guid CardId { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public Guid? NewCardId { get; set; }
    public string? NewCardNumber { get; set; }
    public DateTime OldExpiryDate { get; set; }
    public DateTime NewExpiryDate { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Card network status
/// </summary>
public class CardNetworkStatus
{
    public CardNetwork Network { get; set; }
    public bool IsOnline { get; set; }
    public DateTime LastStatusCheck { get; set; }
    public string? StatusMessage { get; set; }
    public TimeSpan? ResponseTime { get; set; }
}

/// <summary>
/// Batch settlement result
/// </summary>
public class BatchSettlementResult
{
    public bool Success { get; set; }
    public string BatchId { get; set; } = string.Empty;
    public DateTime ProcessedDate { get; set; }
    public int TotalRecords { get; set; }
    public int ProcessedRecords { get; set; }
    public int FailedRecords { get; set; }
    public decimal TotalAmount { get; set; }
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Transaction fees calculation
/// </summary>
public class CardTransactionFees
{
    public decimal InterchangeFee { get; set; }
    public decimal ProcessingFee { get; set; }
    public decimal NetworkFee { get; set; }
    public decimal TotalFees { get; set; }
    public string Currency { get; set; } = "USD";
}

/// <summary>
/// Request for transaction fee calculation
/// </summary>
public class CardTransactionFeeRequest
{
    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    public CardNetwork Network { get; set; }
    
    [Required]
    public CardTransactionType TransactionType { get; set; }
    
    public MerchantCategory MerchantCategory { get; set; }
    
    public bool IsInternational { get; set; }
    
    public bool IsOnline { get; set; }
    
    public string Currency { get; set; } = "USD";
}

#endregion