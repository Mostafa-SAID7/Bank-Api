using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

/// <summary>
/// Represents a loan in the banking system
/// </summary>
public class Loan : BaseEntity
{
    public string LoanNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public User Customer { get; set; } = null!;
    
    // Loan details
    public LoanType Type { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int TermInMonths { get; set; }
    public InterestCalculationMethod CalculationMethod { get; set; } = InterestCalculationMethod.ReducingBalance;
    
    // Status and dates
    public LoanStatus Status { get; set; } = LoanStatus.UnderReview;
    public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;
    public DateTime? ApprovalDate { get; set; }
    public DateTime? DisbursementDate { get; set; }
    public DateTime? MaturityDate { get; set; }
    
    // Financial tracking
    public decimal OutstandingBalance { get; set; }
    public decimal TotalInterestPaid { get; set; }
    public decimal TotalPrincipalPaid { get; set; }
    public decimal MonthlyPaymentAmount { get; set; }
    
    // Credit scoring
    public int? CreditScore { get; set; }
    public CreditScoreRange? CreditScoreRange { get; set; }
    public DateTime? CreditScoringDate { get; set; }
    
    // Delinquency tracking
    public int DaysOverdue { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public DateTime? NextPaymentDueDate { get; set; }
    
    // Application details
    public string Purpose { get; set; } = string.Empty;
    public decimal RequestedAmount { get; set; }
    public string? RejectionReason { get; set; }
    public Guid? ApprovedBy { get; set; }
    public User? ApprovedByUser { get; set; }
    
    // Collections
    public ICollection<LoanPayment> Payments { get; set; } = new List<LoanPayment>();
    public ICollection<LoanDocument> Documents { get; set; } = new List<LoanDocument>();
    public ICollection<LoanStatusHistory> StatusHistory { get; set; } = new List<LoanStatusHistory>();
    
    // Domain methods
    public bool IsActive()
    {
        return Status == LoanStatus.Active || Status == LoanStatus.Disbursed;
    }
    
    public bool IsOverdue()
    {
        return Status == LoanStatus.Delinquent || DaysOverdue > 0;
    }
    
    public bool IsEligibleForDisbursement()
    {
        return Status == LoanStatus.Approved && !DisbursementDate.HasValue;
    }
    
    public void MarkAsDelinquent()
    {
        if (IsActive() && DaysOverdue > 30)
        {
            Status = LoanStatus.Delinquent;
        }
    }
    
    public void UpdateOutstandingBalance(decimal paymentAmount, decimal principalPortion, decimal interestPortion)
    {
        OutstandingBalance -= principalPortion;
        TotalPrincipalPaid += principalPortion;
        TotalInterestPaid += interestPortion;
        LastPaymentDate = DateTime.UtcNow;
        
        if (OutstandingBalance <= 0)
        {
            OutstandingBalance = 0;
            Status = LoanStatus.PaidOff;
        }
    }
    
    public decimal CalculateMonthlyPayment()
    {
        if (TermInMonths <= 0 || PrincipalAmount <= 0 || InterestRate <= 0)
            return 0;
            
        var monthlyRate = (double)(InterestRate / 100) / 12;
        var numberOfPayments = TermInMonths;
        
        // EMI calculation: P * r * (1 + r)^n / ((1 + r)^n - 1)
        var emi = PrincipalAmount * (decimal)(monthlyRate * Math.Pow(1 + monthlyRate, numberOfPayments) / 
                  (Math.Pow(1 + monthlyRate, numberOfPayments) - 1));
                  
        return Math.Round(emi, 2);
    }
    
    public void GenerateRepaymentSchedule()
    {
        if (!IsEligibleForDisbursement() && Status != LoanStatus.Disbursed)
            return;
            
        MonthlyPaymentAmount = CalculateMonthlyPayment();
        
        if (DisbursementDate.HasValue)
        {
            NextPaymentDueDate = DisbursementDate.Value.AddMonths(1);
            MaturityDate = DisbursementDate.Value.AddMonths(TermInMonths);
        }
    }
}