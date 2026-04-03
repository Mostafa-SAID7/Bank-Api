using Bank.Application.DTOs.Deposit.Interest;
using FluentValidation;

namespace Bank.Application.Validators.Deposit;

/// <summary>
/// Validator for interest calculation requests
/// </summary>
public class InterestCalculationRequestValidator : AbstractValidator<InterestCalculationRequest>
{
    public InterestCalculationRequestValidator()
    {
        RuleFor(x => x.Principal)
            .GreaterThan(0)
            .WithMessage("Principal amount must be greater than zero");

        RuleFor(x => x.InterestRate)
            .GreaterThan(0)
            .WithMessage("Interest rate must be greater than zero")
            .LessThanOrEqualTo(100)
            .WithMessage("Interest rate cannot exceed 100%");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required")
            .LessThan(x => x.EndDate)
            .WithMessage("Start date must be before end date");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End date is required")
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("End date cannot be in the future");

        // Validate date range is reasonable (max 50 years)
        RuleFor(x => x)
            .Must(x => (x.EndDate - x.StartDate).TotalDays <= 18250)
            .WithMessage("Calculation period cannot exceed 50 years")
            .WithName("DateRange");

        // Validate calculation period is at least 1 day
        RuleFor(x => x)
            .Must(x => (x.EndDate - x.StartDate).TotalDays >= 1)
            .WithMessage("Calculation period must be at least 1 day")
            .WithName("MinimumPeriod");
    }
}
