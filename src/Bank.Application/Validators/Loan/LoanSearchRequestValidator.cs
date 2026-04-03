using Bank.Application.DTOs.Loan;
using FluentValidation;

namespace Bank.Application.Validators.Loan;

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
