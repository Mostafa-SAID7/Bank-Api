using Bank.Domain.Enums;

namespace Bank.Application.DTOs;

/// <summary>
/// DTO for loan application request
/// </summary>
public class LoanApplicationRequest
{
    public LoanType Type { get; set; }
    public decimal RequestedAmount { get; set; }
    public int TermInMonths { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public decimal MonthlyIncome { get; set; }
    public decimal ExistingDebtAmount { get; set; }
    public string EmploymentStatus { get; set; } = string.Empty;
    public int EmploymentYears { get; set; }
    public string? CollateralDescription { get; set; }
    public decimal? CollateralValue { get; set; }
}

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

/// <summary>
/// DTO for credit scoring result
/// </summary>
public class CreditScoreResult
{
    public bool IsSuccess { get; set; }
    public int CreditScore { get; set; }
    public CreditScoreRange ScoreRange { get; set; }
    public string RiskAssessment { get; set; } = string.Empty;
    public decimal RecommendedInterestRate { get; set; }
    public decimal MaxLoanAmount { get; set; }
    public List<string> RiskFactors { get; set; } = new();
    public DateTime ScoringDate { get; set; }
}

/// <summary>
/// DTO for loan approval decision
/// </summary>
public class ApprovalDecision
{
    public bool IsApproved { get; set; }
    public decimal? ApprovedAmount { get; set; }
    public decimal? InterestRate { get; set; }
    public int? ApprovedTermInMonths { get; set; }
    public string? Conditions { get; set; }
    public string? RejectionReason { get; set; }
    public List<string> RequiredDocuments { get; set; } = new();
}

/// <summary>
/// DTO for loan approval result
/// </summary>
public class LoanApprovalResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public LoanStatus NewStatus { get; set; }
    public decimal? ApprovedAmount { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? MonthlyPayment { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public List<string> NextSteps { get; set; } = new();
}

/// <summary>
/// DTO for loan disbursement result
/// </summary>
public class DisbursementResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public decimal DisbursedAmount { get; set; }
    public string? TransactionReference { get; set; }
    public DateTime DisbursementDate { get; set; }
    public DateTime FirstPaymentDueDate { get; set; }
    public decimal MonthlyPaymentAmount { get; set; }
}

/// <summary>
/// DTO for loan payment result
/// </summary>
public class PaymentResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public decimal PaymentAmount { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestAmount { get; set; }
    public decimal RemainingBalance { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? TransactionReference { get; set; }
    public DateTime? NextPaymentDueDate { get; set; }
}

/// <summary>
/// DTO for loan details
/// </summary>
public class LoanDto
{
    public Guid Id { get; set; }
    public string LoanNumber { get; set; } = string.Empty;
    public LoanType Type { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int TermInMonths { get; set; }
    public LoanStatus Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public DateTime ApplicationDate { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public DateTime? DisbursementDate { get; set; }
    public DateTime? MaturityDate { get; set; }
    public decimal OutstandingBalance { get; set; }
    public decimal MonthlyPaymentAmount { get; set; }
    public DateTime? NextPaymentDueDate { get; set; }
    public int DaysOverdue { get; set; }
    public decimal TotalInterestPaid { get; set; }
    public decimal TotalPrincipalPaid { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public int? CreditScore { get; set; }
    public CreditScoreRange? CreditScoreRange { get; set; }
}

/// <summary>
/// DTO for repayment schedule entry
/// </summary>
public class RepaymentScheduleEntry
{
    public int PaymentNumber { get; set; }
    public DateTime DueDate { get; set; }
    public decimal PaymentAmount { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestAmount { get; set; }
    public decimal RemainingBalance { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaidDate { get; set; }
}

/// <summary>
/// DTO for complete repayment schedule
/// </summary>
public class RepaymentSchedule
{
    public Guid LoanId { get; set; }
    public string LoanNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal TotalInterest { get; set; }
    public decimal MonthlyPayment { get; set; }
    public int TotalPayments { get; set; }
    public List<RepaymentScheduleEntry> Schedule { get; set; } = new();
}

/// <summary>
/// DTO for loan payment request
/// </summary>
public class LoanPaymentRequest
{
    public Guid LoanId { get; set; }
    public decimal PaymentAmount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

/// <summary>
/// DTO for loan search and filtering
/// </summary>
public class LoanSearchRequest
{
    public Guid? CustomerId { get; set; }
    public LoanType? Type { get; set; }
    public LoanStatus? Status { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public DateTime? ApplicationDateFrom { get; set; }
    public DateTime? ApplicationDateTo { get; set; }
    public bool? IsOverdue { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortBy { get; set; } = "ApplicationDate";
    public bool SortDescending { get; set; } = true;
}
/// <summary>
/// DTO for loan interest calculation result
/// </summary>
public class LoanInterestCalculationResult
{
    public decimal InterestAmount { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal TotalPayment { get; set; }
    public decimal RemainingBalance { get; set; }
    public InterestCalculationMethod CalculationMethod { get; set; }
    public DateTime CalculationDate { get; set; }
    public int DaysCalculated { get; set; }
}

/// <summary>
/// DTO for amortization schedule
/// </summary>
public class AmortizationSchedule
{
    public Guid LoanId { get; set; }
    public string LoanNumber { get; set; } = string.Empty;
    public decimal TotalInterest { get; set; }
    public decimal TotalPayments { get; set; }
    public List<AmortizationEntry> Schedule { get; set; } = new();
}

/// <summary>
/// DTO for amortization schedule entry
/// </summary>
public class AmortizationEntry
{
    public int PaymentNumber { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal PaymentAmount { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestAmount { get; set; }
    public decimal RemainingBalance { get; set; }
    public decimal CumulativeInterest { get; set; }
    public decimal CumulativePrincipal { get; set; }
}

/// <summary>
/// DTO for early payoff calculation
/// </summary>
public class EarlyPayoffCalculation
{
    public Guid LoanId { get; set; }
    public DateTime PayoffDate { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal AccruedInterest { get; set; }
    public decimal PrepaymentPenalty { get; set; }
    public decimal TotalPayoffAmount { get; set; }
    public decimal InterestSavings { get; set; }
    public int PaymentsSaved { get; set; }
}

/// <summary>
/// DTO for loan type configuration
/// </summary>
public class LoanTypeConfiguration
{
    public LoanType LoanType { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public decimal MinimumAmount { get; set; }
    public decimal MaximumAmount { get; set; }
    public int MinimumTermMonths { get; set; }
    public int MaximumTermMonths { get; set; }
    public decimal BaseInterestRate { get; set; }
    public InterestCalculationMethod DefaultCalculationMethod { get; set; }
    public bool RequiresCollateral { get; set; }
    public decimal MaxLoanToValueRatio { get; set; }
    public List<string> RequiredDocuments { get; set; } = new();
    public decimal ProcessingFeePercentage { get; set; }
    public decimal PrepaymentPenaltyPercentage { get; set; }
    public bool AllowsEarlyPayoff { get; set; }
}

/// <summary>
/// DTO for loan analytics and reporting
/// </summary>
public class LoanAnalyticsDto
{
    public int TotalLoans { get; set; }
    public decimal TotalLoanAmount { get; set; }
    public decimal TotalOutstandingBalance { get; set; }
    public decimal AverageInterestRate { get; set; }
    public int AverageTermMonths { get; set; }
    public Dictionary<LoanType, int> LoansByType { get; set; } = new();
    public Dictionary<LoanStatus, int> LoansByStatus { get; set; } = new();
    public Dictionary<CreditScoreRange, int> LoansByCreditScore { get; set; } = new();
    public decimal DelinquencyRate { get; set; }
    public decimal DefaultRate { get; set; }
    public DateTime ReportDate { get; set; }
}

/// <summary>
/// DTO for loan performance metrics
/// </summary>
public class LoanPerformanceMetrics
{
    public Guid LoanId { get; set; }
    public string LoanNumber { get; set; } = string.Empty;
    public decimal PaymentToIncomeRatio { get; set; }
    public decimal DebtToIncomeRatio { get; set; }
    public int PaymentHistory { get; set; } // Number of on-time payments
    public int MissedPayments { get; set; }
    public decimal TotalInterestPaid { get; set; }
    public decimal PercentagePaid { get; set; }
    public DateTime LastPaymentDate { get; set; }
    public LoanRiskLevel RiskLevel { get; set; }
}

/// <summary>
/// Enum for loan risk levels
/// </summary>
public enum LoanRiskLevel
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}
/// <summary>
/// DTO for portfolio risk metrics
/// </summary>
public class PortfolioRiskMetrics
{
    public decimal TotalExposure { get; set; }
    public decimal WeightedAverageRisk { get; set; }
    public decimal ConcentrationRisk { get; set; }
    public Dictionary<LoanType, decimal> RiskByType { get; set; } = new();
    public Dictionary<CreditScoreRange, decimal> RiskByScore { get; set; } = new();
    public decimal VaR95 { get; set; } // Value at Risk 95%
    public decimal ExpectedLoss { get; set; }
    public DateTime CalculationDate { get; set; }
}

/// <summary>
/// DTO for loan origination trends
/// </summary>
public class LoanOriginationTrend
{
    public DateTime Month { get; set; }
    public int LoansOriginated { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AverageAmount { get; set; }
    public decimal AverageInterestRate { get; set; }
    public Dictionary<LoanType, int> OriginationsByType { get; set; } = new();
}

/// <summary>
/// DTO for customer loan summary
/// </summary>
public class CustomerLoanSummary
{
    public Guid CustomerId { get; set; }
    public int TotalLoans { get; set; }
    public int ActiveLoans { get; set; }
    public decimal TotalBorrowed { get; set; }
    public decimal TotalOutstanding { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal WeightedAverageRate { get; set; }
    public int OnTimePayments { get; set; }
    public int LatePayments { get; set; }
    public decimal PaymentReliabilityScore { get; set; }
    public LoanRiskLevel RiskLevel { get; set; }
    public List<LoanDto> Loans { get; set; } = new();
}
/// <summary>
/// Request DTO for monthly payment calculation
/// </summary>
public class MonthlyPaymentRequest
{
    public decimal Principal { get; set; }
    public decimal AnnualRate { get; set; }
    public int TermInMonths { get; set; }
    public InterestCalculationMethod CalculationMethod { get; set; } = InterestCalculationMethod.ReducingBalance;
}

/// <summary>
/// Request DTO for early payoff calculation
/// </summary>
public class EarlyPayoffRequest
{
    public DateTime PayoffDate { get; set; }
}

/// <summary>
/// Request DTO for interest rate inquiry
/// </summary>
public class InterestRateRequest
{
    public LoanType LoanType { get; set; }
    public int CreditScore { get; set; }
    public decimal LoanAmount { get; set; }
}

/// <summary>
/// Request DTO for updating loan interest rate
/// </summary>
public class UpdateLoanRateRequest
{
    public decimal NewRate { get; set; }
}