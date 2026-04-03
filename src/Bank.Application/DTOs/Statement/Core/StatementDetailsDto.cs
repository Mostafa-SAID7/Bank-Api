using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Statement.Core;

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

