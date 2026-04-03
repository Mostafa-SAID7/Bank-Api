using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

public class PaymentTemplate : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public Guid FromAccountId { get; set; }
    public Account FromAccount { get; set; } = null!;
    
    public Guid? ToAccountId { get; set; } // Optional for templates
    public Account? ToAccount { get; set; }
    
    public string? BeneficiaryName { get; set; }
    public string? BeneficiaryAccountNumber { get; set; }
    public string? BeneficiaryBankCode { get; set; }
    
    public decimal? Amount { get; set; } // Optional for templates
    public TransactionType Type { get; set; }
    public string? Reference { get; set; }
    public string? Notes { get; set; }
    
    // Template metadata
    public bool IsActive { get; set; } = true;
    public int UsageCount { get; set; } = 0;
    public DateTime? LastUsedDate { get; set; }
    
    // Created by
    public Guid CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = null!;
    
    // Template categories for organization
    public PaymentTemplateCategory Category { get; set; } = PaymentTemplateCategory.General;
    public string? Tags { get; set; } // JSON array of tags
    
    public void RecordUsage()
    {
        UsageCount++;
        LastUsedDate = DateTime.UtcNow;
    }
    
    public void Deactivate()
    {
        IsActive = false;
    }
    
    public void Activate()
    {
        IsActive = true;
    }
}