using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Payment.Beneficiary;

/// <summary>
/// Beneficiary search criteria
/// </summary>
public class BeneficiarySearchCriteria
{
    public Guid CustomerId { get; set; }
    public string? SearchTerm { get; set; }
    public string? AccountNumber { get; set; }
    public string? Name { get; set; }
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

