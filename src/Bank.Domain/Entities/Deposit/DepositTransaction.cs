using Bank.Domain.Common;
using Bank.Domain.Enums;
using System.Security.Cryptography;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents transactions related to deposit products (interest credits, penalties, etc.)
/// </summary>
public class DepositTransaction : BaseEntity
{
    public Guid DepositId { get; set; }
    public Guid FixedDepositId { get; set; }
    public string TransactionReference { get; set; } = string.Empty;
    public DepositTransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public TransactionStatus Status { get; set; }
    
    // Interest-specific fields
    public DateTime? InterestPeriodStart { get; set; }
    public DateTime? InterestPeriodEnd { get; set; }
    public decimal? InterestRate { get; set; }
    public int? InterestDays { get; set; }
    
    // Penalty-specific fields
    public WithdrawalPenaltyType? PenaltyType { get; set; }
    public string? PenaltyReason { get; set; }
    
    // Processing details
    public Guid? ProcessedByUserId { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public string? ProcessingNotes { get; set; }
    
    // Related transaction (for reversals, adjustments)
    public Guid? RelatedTransactionId { get; set; }
    public Guid? AccountTransactionId { get; set; }
    
    // Navigation properties
    public virtual FixedDeposit FixedDeposit { get; set; } = null!;
    public virtual User? ProcessedByUser { get; set; }
    public virtual DepositTransaction? RelatedTransaction { get; set; }
    public virtual Transaction? AccountTransaction { get; set; }
    
    /// <summary>
    /// Generates a unique transaction reference
    /// </summary>
    public void GenerateTransactionReference()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[4];
        rng.GetBytes(randomBytes);
        var random = Math.Abs(BitConverter.ToInt32(randomBytes, 0)) % 900 + 100;
        TransactionReference = $"DT{timestamp}{random}";
    }
}
