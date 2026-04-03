using Bank.Application.DTOs.Deposit.Interest;
using Bank.Domain.Enums;
using FluentValidation;

namespace Bank.Application.Validators.Deposit;

/// <summary>
/// Validator for apply interest requests
/// </summary>
public class ApplyInterestRequestValidator : AbstractValidator<ApplyInterestRequest>
{
    public ApplyInterestRequestValidator()
    {
        RuleFor(x => x.DepositId)
            .NotEmpty()
            .WithMessage("Deposit ID is required");

        RuleFor(x => x.InterestRate)
            .GreaterThan(0)
            .WithMessage("Interest rate must be greater than zero")
            .LessThanOrEqualTo(100)
            .WithMessage("Interest rate cannot exceed 100%");

        RuleFor(x => x.CalculationMethod)
            .IsInEnum()
            .WithMessage("Invalid interest calculation method");

        RuleFor(x => x.CompoundingFrequency)
            .IsInEnum()
            .WithMessage("Invalid compounding frequency");

        RuleFor(x => x.EffectiveDate)
            .NotEmpty()
            .WithMessage("Effective date is required")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Effective date cannot be in the future");

        // Validate calculation method specific rules
        When(x => x.CalculationMethod == InterestCalculationMethod.Simple, () =>
        {
            RuleFor(x => x.CompoundingFrequency)
                .Equal(CompoundingFrequency.None)
                .WithMessage("Simple interest does not use compounding frequency");
        });

        When(x => x.CalculationMethod == InterestCalculationMethod.Compound, () =>
        {
            RuleFor(x => x.CompoundingFrequency)
                .NotEqual(CompoundingFrequency.None)
                .WithMessage("Compound interest requires a compounding frequency");
        });
    }
}
