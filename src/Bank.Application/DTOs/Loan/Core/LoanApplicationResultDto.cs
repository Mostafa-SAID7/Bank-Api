using Bank.Domain.Enums;

namespace Bank.Application.DTOs.Loan.Core;

/// <summary>
/// DTO for loan application result
/// </summary>
public class LoanApplicationResult
{
    public bool IsSuccess { get; set; }
    public string LoanNumber { get; set; } = string.Empty;
    public Guid LoanId { get; set; }
    public LoanStatus Status { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> RequiredDocuments { get; set; } = new();
    public DateTime ApplicationDate { get; set; }
}

