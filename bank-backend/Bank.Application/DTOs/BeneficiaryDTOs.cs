using Bank.Domain.Enums;

namespace Bank.Application.DTOs;

/// <summary>
/// Request to add a new beneficiary
/// </summary>
public class AddBeneficiaryRequest
{
    public string Name { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string? AccountName { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string BankCode { get; set; } = string.Empty;
    public string? SwiftCode { get; set; }
    public string? IbanNumber { get; set; }
    public string? RoutingNumber { get; set; }
    public BeneficiaryType Type { get; set; }
    public BeneficiaryCategory Category { get; set; }
    public decimal? DailyTransferLimit { get; set; }
    public decimal? MonthlyTransferLimit { get; set; }
    public decimal? SingleTransferLimit { get; set; }
    public string? Notes { get; set; }
    public string? Reference { get; set; }
}

/// <summary>
/// Request to update beneficiary information
/// </summary>
public class UpdateBeneficiaryRequest
{
    public string? Name { get; set; }
    public string? AccountName { get; set; }
    public string? BankName { get; set; }
    public BeneficiaryCategory? Category { get; set; }
    public decimal? DailyTransferLimit { get; set; }
    public decimal? MonthlyTransferLimit { get; set; }
    public decimal? SingleTransferLimit { get; set; }
    public string? Notes { get; set; }
    public string? Reference { get; set; }
}

/// <summary>
/// Beneficiary data transfer object
/// </summary>
public class BeneficiaryDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string? AccountName { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string BankCode { get; set; } = string.Empty;
    public string? SwiftCode { get; set; }
    public string? IbanNumber { get; set; }
    public string? RoutingNumber { get; set; }
    public BeneficiaryType Type { get; set; }
    public BeneficiaryCategory Category { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? VerifiedDate { get; set; }
    public BeneficiaryStatus Status { get; set; }
    public decimal? DailyTransferLimit { get; set; }
    public decimal? MonthlyTransferLimit { get; set; }
    public decimal? SingleTransferLimit { get; set; }
    public bool IsActive { get; set; }
    public string? Notes { get; set; }
    public string? Reference { get; set; }
    public DateTime? LastTransferDate { get; set; }
    public int TransferCount { get; set; }
    public decimal TotalTransferAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Result of beneficiary operations
/// </summary>
public class BeneficiaryResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public BeneficiaryDto? Beneficiary { get; set; }
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Result of beneficiary verification
/// </summary>
public class BeneficiaryVerificationResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public bool IsAccountValid { get; set; }
    public string? AccountHolderName { get; set; }
    public string? BankName { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
}

/// <summary>
/// Request for beneficiary verification
/// </summary>
public class VerifyBeneficiaryRequest
{
    public Guid BeneficiaryId { get; set; }
    public bool ForceVerification { get; set; } = false;
}

/// <summary>
/// Beneficiary search criteria
/// </summary>
public class BeneficiarySearchCriteria
{
    public Guid CustomerId { get; set; }
    public string? Name { get; set; }
    public string? AccountNumber { get; set; }
    public string? BankCode { get; set; }
    public BeneficiaryType? Type { get; set; }
    public BeneficiaryCategory? Category { get; set; }
    public BeneficiaryStatus? Status { get; set; }
    public bool? IsVerified { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

/// <summary>
/// Beneficiary search result
/// </summary>
public class BeneficiarySearchResult
{
    public List<BeneficiaryDto> Beneficiaries { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}

/// <summary>
/// Beneficiary transfer history
/// </summary>
public class BeneficiaryTransferHistory
{
    public Guid BeneficiaryId { get; set; }
    public string BeneficiaryName { get; set; } = string.Empty;
    public List<TransferHistoryItem> Transfers { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public int TotalCount { get; set; }
}

/// <summary>
/// Transfer history item
/// </summary>
public class TransferHistoryItem
{
    public Guid TransactionId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransferDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public TransactionStatus Status { get; set; }
}

/// <summary>
/// Beneficiary statistics
/// </summary>
public class BeneficiaryStatistics
{
    public int TotalBeneficiaries { get; set; }
    public int ActiveBeneficiaries { get; set; }
    public int VerifiedBeneficiaries { get; set; }
    public int PendingVerification { get; set; }
    public Dictionary<BeneficiaryCategory, int> BeneficiariesByCategory { get; set; } = new();
    public Dictionary<BeneficiaryType, int> BeneficiariesByType { get; set; } = new();
    public decimal TotalTransferAmount { get; set; }
    public int TotalTransfers { get; set; }
}