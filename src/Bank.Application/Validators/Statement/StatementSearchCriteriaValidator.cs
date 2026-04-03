using Bank.Application.DTOs.Statement.Search;
using FluentValidation;

namespace Bank.Application.Validators.Statement;

/// <summary>
/// Validator for statement search criteria
/// </summary>
public class StatementSearchCriteriaValidator : AbstractValidator<StatementSearchCriteria>
{
    public StatementSearchCriteriaValidator()
    {
        // Validate date range if provided
        When(x => x.FromDate.HasValue || x.ToDate.HasValue, () =>
        {
            RuleFor(x => x.FromDate)
                .LessThan(x => x.ToDate)
                .WithMessage("From date must be before to date")
                .When(x => x.FromDate.HasValue && x.ToDate.HasValue);

            RuleFor(x => x.ToDate)
                .GreaterThan(x => x.FromDate)
                .WithMessage("To date must be after from date")
                .When(x => x.FromDate.HasValue && x.ToDate.HasValue);

            RuleFor(x => x.ToDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("To date cannot be in the future")
                .When(x => x.ToDate.HasValue);

            // Validate date range is not too large (max 5 years)
            RuleFor(x => x)
                .Must(x => !x.FromDate.HasValue || !x.ToDate.HasValue || (x.ToDate.Value - x.FromDate.Value).TotalDays <= 1825)
                .WithMessage("Date range cannot exceed 5 years")
                .WithName("DateRange");
        });

        // Validate pagination
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size cannot exceed 100");

        // Validate status if provided
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid statement status")
            .When(x => x.Status.HasValue);

        // Validate format if provided
        RuleFor(x => x.Format)
            .IsInEnum()
            .WithMessage("Invalid statement format")
            .When(x => x.Format.HasValue);
    }
}
