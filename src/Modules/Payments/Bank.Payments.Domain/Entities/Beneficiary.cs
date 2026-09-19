using Bank.Payments.Domain.Enums;

namespace Bank.Payments.Domain.Entities;

/// <summary>
/// Represents a beneficiary (payee) for fund transfers
/// Migrated from Bank.Domain - owned by Payments module
/// </summary>
public class Beneficiary
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Nickname { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string? AccountName { get; set; }
    
    // Bank details
    public string BankName { get; set; } = string.Empty;
    public string BankCode { get; set; } = string.Empty;
    public string? SwiftCode { get; set; }
    public string? IbanNumber { get; set; }
    public string? RoutingNumber { get; set; }
    
    // Classification
    public BeneficiaryType Type { get; set; } = BeneficiaryType.External;
    public BeneficiaryCategory Category { get; set; } = BeneficiaryCategory.Personal;
    
    // Verification and status
    public bool IsVerified { get; set; }
    public DateTime? VerifiedDate { get; set; }
    public Guid? VerifiedByUserId { get; set; }
    public BeneficiaryStatus Status { get; set; } = BeneficiaryStatus.Pending;
    
    // Transfer limits
    public decimal? DailyTransferLimit { get; set; }
    public decimal? MonthlyTransferLimit { get; set; }
    public decimal? SingleTransferLimit { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Metadata
    public string? Notes { get; set; }
    public string? Reference { get; set; }
    public DateTime? LastTransferDate { get; set; }
    public decimal? LastTransferAmount { get; set; }
    public int TransferCount { get; set; }
    public decimal TotalTransferAmount { get; set; }
    public DateTime? ArchivedDate { get; set; }
    public string? ArchiveReason { get; set; }
    
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    private Beneficiary() { }

    public static Beneficiary Create(
        Guid customerId,
        string name,
        string accountNumber,
        string bankName,
        string bankCode,
        BeneficiaryType type = BeneficiaryType.External,
        BeneficiaryCategory category = BeneficiaryCategory.Personal)
    {
        return new Beneficiary
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            Name = name,
            AccountNumber = accountNumber,
            BankName = bankName,
            BankCode = bankCode,
            Type = type,
            Category = category,
            IsActive = true,
            Status = BeneficiaryStatus.Pending,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public void Verify(Guid verifiedByUserId)
    {
        IsVerified = true;
        VerifiedDate = DateTime.UtcNow;
        VerifiedByUserId = verifiedByUserId;
        Status = BeneficiaryStatus.Active;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Archive(string reason)
    {
        IsActive = false;
        Status = BeneficiaryStatus.Archived;
        ArchivedDate = DateTime.UtcNow;
        ArchiveReason = reason;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Reactivate()
    {
        IsActive = true;
        Status = BeneficiaryStatus.Active;
        ArchivedDate = null;
        ArchiveReason = null;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void RecordTransfer(decimal amount)
    {
        LastTransferDate = DateTime.UtcNow;
        LastTransferAmount = amount;
        TransferCount++;
        TotalTransferAmount += amount;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
