using Bank.Domain.Enums;

namespace Bank.Application.DTOs;

/// <summary>
/// Request to generate an account statement
/// </summary>
public class GenerateStatementRequest
{
    public Guid AccountId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public StatementFormat Format { get; set; } = StatementFormat.PDF;
    public StatementDeliveryMethod DeliveryMethod { get; set; } = StatementDeliveryMethod.Email;
    public string? EmailAddress { get; set; }
    public bool IncludeTransactionDetails { get; set; } = true;
    public bool IncludeSummary { get; set; } = true;
    public bool IncludeCharts { get; set; } = false;
    public string? CustomTitle { get; set; }
    public List<TransactionType>? FilterByTransactionTypes { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public string? FilterByDescription { get; set; }
    public List<string>? FilterByCategories { get; set; }
    public string? RequestReason { get; set; }
}

/// <summary>
/// Statement generation result
/// </summary>
public class StatementGenerationResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public Guid? StatementId { get; set; }
    public string? StatementNumber { get; set; }
    public string? FileName { get; set; }
    public string? FilePath { get; set; }
    public long? FileSizeBytes { get; set; }
    public string? DownloadUrl { get; set; }
    public DateTime? GeneratedDate { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
}

/// <summary>
/// Statement data transfer object
/// </summary>
public class StatementDto
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountHolderName { get; set; } = string.Empty;
    public DateTime StatementDate { get; set; }
    public DateTime PeriodStartDate { get; set; }
    public DateTime PeriodEndDate { get; set; }
    public string StatementNumber { get; set; } = string.Empty;
    public decimal OpeningBalance { get; set; }
    public decimal ClosingBalance { get; set; }
    public decimal AverageBalance { get; set; }
    public int TotalTransactions { get; set; }
    public decimal TotalDebits { get; set; }
    public decimal TotalCredits { get; set; }
    public decimal TotalFees { get; set; }
    public decimal InterestEarned { get; set; }
    public StatementStatus Status { get; set; }
    public StatementFormat Format { get; set; }
    public string? FileName { get; set; }
    public long? FileSizeBytes { get; set; }
    public bool IsDelivered { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Statement transaction DTO
/// </summary>
public class StatementTransactionDto
{
    public Guid Id { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public decimal Amount { get; set; }
    public decimal RunningBalance { get; set; }
    public TransactionType Type { get; set; }
    public TransactionStatus Status { get; set; }
    public string? Category { get; set; }
    public string? Memo { get; set; }
}

/// <summary>
/// Statement search criteria
/// </summary>
public class StatementSearchCriteria
{
    public Guid? AccountId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public StatementStatus? Status { get; set; }
    public StatementFormat? Format { get; set; }
    public bool? IsDelivered { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

/// <summary>
/// Statement search result
/// </summary>
public class StatementSearchResult
{
    public List<StatementDto> Statements { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}

/// <summary>
/// Statement summary for dashboard
/// </summary>
public class StatementSummary
{
    public Guid AccountId { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal ClosingBalance { get; set; }
    public decimal NetChange { get; set; }
    public int TransactionCount { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal TotalFees { get; set; }
    public decimal InterestEarned { get; set; }
    public List<TransactionCategorySummary> CategoryBreakdown { get; set; } = new();
    public List<MonthlyTransactionSummary> MonthlyBreakdown { get; set; } = new();
}

/// <summary>
/// Transaction category summary
/// </summary>
public class TransactionCategorySummary
{
    public string Category { get; set; } = string.Empty;
    public int TransactionCount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Percentage { get; set; }
}

/// <summary>
/// Monthly transaction summary
/// </summary>
public class MonthlyTransactionSummary
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public int TransactionCount { get; set; }
    public decimal TotalDebits { get; set; }
    public decimal TotalCredits { get; set; }
    public decimal NetAmount { get; set; }
    public decimal AverageBalance { get; set; }
}

/// <summary>
/// Consolidated statement request for multiple accounts
/// </summary>
public class ConsolidatedStatementRequest
{
    public List<Guid> AccountIds { get; set; } = new();
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public StatementFormat Format { get; set; } = StatementFormat.PDF;
    public StatementDeliveryMethod DeliveryMethod { get; set; } = StatementDeliveryMethod.Email;
    public string? EmailAddress { get; set; }
    public bool IncludeAccountSummaries { get; set; } = true;
    public bool IncludeConsolidatedSummary { get; set; } = true;
    public bool IncludeTransactionDetails { get; set; } = true;
    public string? CustomTitle { get; set; }
}

/// <summary>
/// Statement delivery status
/// </summary>
public class StatementDeliveryStatus
{
    public Guid StatementId { get; set; }
    public bool IsDelivered { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public string? DeliveryReference { get; set; }
    public StatementDeliveryMethod DeliveryMethod { get; set; }
    public string? DeliveryAddress { get; set; }
    public List<string> DeliveryAttempts { get; set; } = new();
    public string? LastError { get; set; }
}

/// <summary>
/// Statement statistics for analytics
/// </summary>
public class StatementStatistics
{
    public Guid AccountId { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public int TransactionCount { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetChange { get; set; }
    public List<TransactionCategorySummary> CategoryBreakdown { get; set; } = new();
    public List<MonthlyTransactionSummary> MonthlyBreakdown { get; set; } = new();
}

/// <summary>
/// Statement template configuration
/// </summary>
public class StatementTemplate
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public StatementFormat Format { get; set; }
    public string? LogoPath { get; set; }
    public string? HeaderTemplate { get; set; }
    public string? FooterTemplate { get; set; }
    public Dictionary<string, object> CustomFields { get; set; } = new();
    public bool IncludeBankBranding { get; set; } = true;
    public bool IncludeRegulatoryDisclosures { get; set; } = true;
    public string? CssStyles { get; set; }
}
/// <summary>
/// Request model for delivering statements
/// </summary>
public class DeliverStatementRequest
{
    public StatementDeliveryMethod DeliveryMethod { get; set; }
    public string DeliveryAddress { get; set; } = string.Empty;
}

/// <summary>
/// Validation result model
/// </summary>
public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
}