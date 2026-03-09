using Bank.Application.DTOs;
using Bank.Domain.Enums;
using FluentValidation;

namespace Bank.Application.Validators;

/// <summary>
/// Validator for loan application requests
/// </summary>
public class LoanApplicationRequestValidator : AbstractValidator<LoanApplicationRequest>
{
    public LoanApplicationRequestValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid loan type");

        RuleFor(x => x.RequestedAmount)
            .GreaterThan(0)
            .WithMessage("Requested amount must be greater than zero")
            .LessThanOrEqualTo(10_000_000)
            .WithMessage("Requested amount cannot exceed $10,000,000");

        RuleFor(x => x.TermInMonths)
            .GreaterThan(0)
            .WithMessage("Term must be greater than zero")
            .LessThanOrEqualTo(360)
            .WithMessage("Term cannot exceed 360 months (30 years)");

        RuleFor(x => x.Purpose)
            .NotEmpty()
            .WithMessage("Purpose is required")
            .MaximumLength(500)
            .WithMessage("Purpose cannot exceed 500 characters");

        RuleFor(x => x.MonthlyIncome)
            .GreaterThan(0)
            .WithMessage("Monthly income must be greater than zero");

        RuleFor(x => x.ExistingDebtAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Existing debt amount cannot be negative");

        RuleFor(x => x.EmploymentStatus)
            .NotEmpty()
            .WithMessage("Employment status is required")
            .MaximumLength(100)
            .WithMessage("Employment status cannot exceed 100 characters");

        RuleFor(x => x.EmploymentYears)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Employment years cannot be negative")
            .LessThanOrEqualTo(50)
            .WithMessage("Employment years cannot exceed 50");

        // Conditional validation for collateral
        When(x => x.Type == LoanType.Auto || x.Type == LoanType.Mortgage, () =>
        {
            RuleFor(x => x.CollateralDescription)
                .NotEmpty()
                .WithMessage("Collateral description is required for secured loans");

            RuleFor(x => x.CollateralValue)
                .GreaterThan(0)
                .WithMessage("Collateral value must be greater than zero for secured loans");
        });

        // Business logic validations
        RuleFor(x => x)
            .Must(HaveReasonableDebtToIncomeRatio)
            .WithMessage("Debt-to-income ratio is too high (maximum 43%)")
            .WithName("DebtToIncomeRatio");

        RuleFor(x => x)
            .Must(HaveReasonableLoanToValueRatio)
            .WithMessage("Loan-to-value ratio is too high (maximum 80%)")
            .WithName("LoanToValueRatio")
            .When(x => x.CollateralValue.HasValue && x.CollateralValue > 0);
    }

    private static bool HaveReasonableDebtToIncomeRatio(LoanApplicationRequest request)
    {
        if (request.MonthlyIncome <= 0) return false;

        // Estimate monthly payment (simplified calculation)
        var estimatedMonthlyPayment = request.RequestedAmount * 0.01m; // Rough estimate
        var totalMonthlyDebt = (request.ExistingDebtAmount / 12) + estimatedMonthlyPayment;
        var debtToIncomeRatio = totalMonthlyDebt / request.MonthlyIncome;

        return debtToIncomeRatio <= 0.43m; // 43% maximum DTI ratio
    }

    private static bool HaveReasonableLoanToValueRatio(LoanApplicationRequest request)
    {
        if (!request.CollateralValue.HasValue || request.CollateralValue <= 0)
            return true; // Skip validation if no collateral

        var loanToValueRatio = request.RequestedAmount / request.CollateralValue.Value;
        return loanToValueRatio <= 0.80m; // 80% maximum LTV ratio
    }
}

/// <summary>
/// Validator for loan payment requests
/// </summary>
public class LoanPaymentRequestValidator : AbstractValidator<LoanPaymentRequest>
{
    public LoanPaymentRequestValidator()
    {
        RuleFor(x => x.LoanId)
            .NotEmpty()
            .WithMessage("Loan ID is required");

        RuleFor(x => x.PaymentAmount)
            .GreaterThan(0)
            .WithMessage("Payment amount must be greater than zero")
            .LessThanOrEqualTo(1_000_000)
            .WithMessage("Payment amount cannot exceed $1,000,000");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty()
            .WithMessage("Payment method is required")
            .MaximumLength(50)
            .WithMessage("Payment method cannot exceed 50 characters");

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .WithMessage("Notes cannot exceed 1000 characters");
    }
}

/// <summary>
/// Validator for approval decisions
/// </summary>
public class ApprovalDecisionValidator : AbstractValidator<ApprovalDecision>
{
    public ApprovalDecisionValidator()
    {
        When(x => x.IsApproved, () =>
        {
            RuleFor(x => x.ApprovedAmount)
                .GreaterThan(0)
                .WithMessage("Approved amount must be greater than zero when loan is approved");

            RuleFor(x => x.InterestRate)
                .GreaterThan(0)
                .WithMessage("Interest rate must be greater than zero when loan is approved")
                .LessThanOrEqualTo(50)
                .WithMessage("Interest rate cannot exceed 50%");

            RuleFor(x => x.ApprovedTermInMonths)
                .GreaterThan(0)
                .WithMessage("Approved term must be greater than zero when loan is approved")
                .LessThanOrEqualTo(360)
                .WithMessage("Approved term cannot exceed 360 months");
        });

        When(x => !x.IsApproved, () =>
        {
            RuleFor(x => x.RejectionReason)
                .NotEmpty()
                .WithMessage("Rejection reason is required when loan is rejected")
                .MaximumLength(1000)
                .WithMessage("Rejection reason cannot exceed 1000 characters");
        });

        RuleFor(x => x.Conditions)
            .MaximumLength(2000)
            .WithMessage("Conditions cannot exceed 2000 characters");
    }
}

/// <summary>
/// Validator for loan search requests
/// </summary>
public class LoanSearchRequestValidator : AbstractValidator<LoanSearchRequest>
{
    public LoanSearchRequestValidator()
    {
        RuleFor(x => x.MinAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum amount cannot be negative")
            .When(x => x.MinAmount.HasValue);

        RuleFor(x => x.MaxAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Maximum amount cannot be negative")
            .When(x => x.MaxAmount.HasValue);

        RuleFor(x => x)
            .Must(x => !x.MinAmount.HasValue || !x.MaxAmount.HasValue || x.MinAmount <= x.MaxAmount)
            .WithMessage("Minimum amount cannot be greater than maximum amount")
            .WithName("AmountRange");

        RuleFor(x => x)
            .Must(x => !x.ApplicationDateFrom.HasValue || !x.ApplicationDateTo.HasValue || x.ApplicationDateFrom <= x.ApplicationDateTo)
            .WithMessage("Application date from cannot be greater than application date to")
            .WithName("DateRange");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than zero");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than zero")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size cannot exceed 100");

        RuleFor(x => x.SortBy)
            .NotEmpty()
            .WithMessage("Sort by field is required")
            .Must(BeValidSortField)
            .WithMessage("Invalid sort field");
    }

    private static bool BeValidSortField(string sortBy)
    {
        var validFields = new[] { "ApplicationDate", "Amount", "Status", "Type", "LoanNumber" };
        return validFields.Contains(sortBy, StringComparer.OrdinalIgnoreCase);
    }
}