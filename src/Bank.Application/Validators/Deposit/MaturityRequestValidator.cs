using Bank.Application.DTOs.Deposit.Maturity;
using Bank.Domain.Enums;
using FluentValidation;

namespace Bank.Application.Validators.Deposit;

/// <summary>
/// Validator for maturity requests
/// </summary>
public class MaturityRequestValidator : AbstractValidator<MaturityDto>
{
    public MaturityRequestValidator()
    {
        RuleFor(x => x.DepositId)
            .NotEmpty()
            .WithMessage("Deposit ID is required");

        RuleFor(x => x.MaturityDate)
            .NotEmpty()
            .WithMessage("Maturity date is required")
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Maturity date must be in the future");

        RuleFor(x => x.MaturityAmount)
            .GreaterThan(0)
            .WithMessage("Maturity amount must be greater than zero");

        RuleFor(x => x.MaturityAction)
            .IsInEnum()
            .WithMessage("Invalid maturity action");

        // Validate renewal options if action is renew
        When(x => x.MaturityAction == MaturityAction.Renew, () =>
        {
            RuleFor(x => x.RenewalTerm)
                .GreaterThan(0)
                .WithMessage("Renewal term must be greater than zero")
                .When(x => x.RenewalTerm.HasValue);

            RuleFor(x => x.RenewalInterestRate)
                .GreaterThan(0)
                .WithMessage("Renewal interest rate must be greater than zero")
                .LessThanOrEqualTo(100)
                .WithMessage("Renewal interest rate cannot exceed 100%")
                .When(x => x.RenewalInterestRate.HasValue);
        });

        // Validate withdrawal options if action is withdraw
        When(x => x.MaturityAction == MaturityAction.Withdraw, () =>
        {
            RuleFor(x => x.WithdrawalAccountId)
                .NotEmpty()
                .WithMessage("Withdrawal account ID is required for withdrawal action");
        });

        // Validate interest options if action is reinvest
        When(x => x.MaturityAction == MaturityAction.ReinvestInterest, () =>
        {
            RuleFor(x => x.InterestReinvestmentType)
                .IsInEnum()
                .WithMessage("Invalid interest reinvestment type")
                .When(x => x.InterestReinvestmentType.HasValue);
        });

        // Validate notification preferences if provided
        RuleFor(x => x.NotificationPreference)
            .IsInEnum()
            .WithMessage("Invalid notification preference")
            .When(x => x.NotificationPreference.HasValue);
    }
}
