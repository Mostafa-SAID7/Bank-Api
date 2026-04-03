using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents a beneficiary (payee) for fund transfers
/// Supports both internal and external bank accounts with validation and categorization
/// </summary>
public class Beneficiary : BaseEntity
{
    public Guid CustomerId { get; set; }
    public User Customer { get; set; } = null!;
    
    public string Name { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string? AccountName { get; set; }
    
    // Bank details
    public string BankName { get; set; } = string.Empty;
    public string BankCode { get; set; } = string.Empty;
    public string? SwiftCode { get; set; } // For international transfers
    public string? IbanNumber { get; set; } // For international transfers
    public string? RoutingNumber { get; set; } // For domestic transfers
    
    // Beneficiary classification
    public BeneficiaryType Type { get; set; } = BeneficiaryType.External;
    public BeneficiaryCategory Category { get; set; } = BeneficiaryCategory.Personal;
    
    // Verification and status
    public bool IsVerified { get; set; } = false;
    public DateTime? VerifiedDate { get; set; }
    public Guid? VerifiedByUserId { get; set; }
    public User? VerifiedByUser { get; set; }
    public BeneficiaryStatus Status { get; set; } = BeneficiaryStatus.Pending;
    
    // Transfer limits and restrictions
    public decimal? DailyTransferLimit { get; set; }
    public decimal? MonthlyTransferLimit { get; set; }
    public decimal? SingleTransferLimit { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Metadata
    public string? Notes { get; set; }
    public string? Reference { get; set; }
    public DateTime? LastTransferDate { get; set; }
    public int TransferCount { get; set; } = 0;
    public decimal TotalTransferAmount { get; set; } = 0;
    
    // Archival
    public DateTime? ArchivedDate { get; set; }
    public string? ArchiveReason { get; set; }
    
    // Navigation properties
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    
    /// <summary>
    /// Verify the beneficiary account
    /// </summary>
    public void Verify(Guid verifiedByUserId)
    {
        IsVerified = true;
        VerifiedDate = DateTime.UtcNow;
        VerifiedByUserId = verifiedByUserId;
        Status = BeneficiaryStatus.Active;
    }
    
    /// <summary>
    /// Archive the beneficiary while preserving transaction history
    /// </summary>
    public void Archive(string reason)
    {
        IsActive = false;
        Status = BeneficiaryStatus.Archived;
        ArchivedDate = DateTime.UtcNow;
        ArchiveReason = reason;
    }
    
    /// <summary>
    /// Reactivate an archived beneficiary
    /// </summary>
    public void Reactivate()
    {
        IsActive = true;
        Status = BeneficiaryStatus.Active;
        ArchivedDate = null;
        ArchiveReason = null;
    }
    
    /// <summary>
    /// Record a successful transfer to this beneficiary
    /// </summary>
    public void RecordTransfer(decimal amount)
    {
        LastTransferDate = DateTime.UtcNow;
        TransferCount++;
        TotalTransferAmount += amount;
    }
    
    /// <summary>
    /// Check if a transfer amount is within limits
    /// </summary>
    public bool IsTransferWithinLimits(decimal amount)
    {
        if (!IsActive || Status != BeneficiaryStatus.Active)
            return false;
            
        if (SingleTransferLimit.HasValue && amount > SingleTransferLimit.Value)
            return false;
            
        // Additional daily/monthly limit checks would require transaction history
        return true;
    }
    
    /// <summary>
    /// Check if beneficiary is ready for transfers
    /// </summary>
    public bool CanReceiveTransfers()
    {
        return IsActive && 
               Status == BeneficiaryStatus.Active && 
               IsVerified;
    }
    
    /// <summary>
    /// Update transfer limits
    /// </summary>
    public void UpdateTransferLimits(decimal? dailyLimit, decimal? monthlyLimit, decimal? singleLimit)
    {
        DailyTransferLimit = dailyLimit;
        MonthlyTransferLimit = monthlyLimit;
        SingleTransferLimit = singleLimit;
    }
}