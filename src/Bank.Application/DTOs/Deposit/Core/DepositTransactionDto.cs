using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Deposit.Core;

/// <summary>
/// Deposit transaction data transfer object
/// </summary>
public class DepositTransactionDto
{
    public Guid Id { get; set; }
    public Guid FixedDepositId { get; set; }
    public string TransactionReference { get; set; } = string.Empty;
    public DepositTransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public TransactionStatus Status { get; set; }
    
    public DateTime? InterestPeriodStart { get; set; }
    public DateTime? InterestPeriodEnd { get; set; }
    public decimal? InterestRate { get; set; }
    public int? InterestDays { get; set; }
    
    public WithdrawalPenaltyType? PenaltyType { get; set; }
    public string? PenaltyReason { get; set; }
}


