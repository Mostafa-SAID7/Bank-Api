using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

public class AccountFee : BaseEntity
{
    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
    
    public FeeType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime CalculatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? AppliedDate { get; set; }
    public bool IsWaived { get; set; } = false;
    public string? WaiverReason { get; set; }
    public Guid? WaivedByUserId { get; set; }
    
    // Fee schedule information
    public FeeFrequency Frequency { get; set; }
    public DateTime? NextCalculationDate { get; set; }
    
    // Reference to the transaction that applied this fee
    public Guid? TransactionId { get; set; }
    public Transaction? Transaction { get; set; }
    
    public void ApplyFee(Guid? transactionId = null)
    {
        if (!IsWaived && AppliedDate == null)
        {
            AppliedDate = DateTime.UtcNow;
            TransactionId = transactionId;
        }
    }
    
    public void WaiveFee(string reason, Guid waivedByUserId)
    {
        IsWaived = true;
        WaiverReason = reason;
        WaivedByUserId = waivedByUserId;
    }
}