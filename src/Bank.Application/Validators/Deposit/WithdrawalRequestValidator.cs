using Bank.Application.DTOs.Deposit.Withdrawal;
using Bank.Domain.Enums;
using FluentValidation;

namespace Bank.Application.Validators.Deposit;

/// <summary>
/// Validator for withdrawal requests
/// </summary>
public class WithdrawalRequestValidator : AbstractValidator<WithdrawalDto>
{
    public WithdrawalRequestValidator()
    {
        RuleFor(x => x.DepositId)
            .NotEmpty()
            .WithMessage("Deposit ID is required");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Withdrawal amount must be greater than zero");

        RuleFor(x => x.WithdrawalDate)
            .NotEmpty()
            .WithMessage("Withdrawal date is required")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Withdrawal date cannot be in the future");

        RuleFor(x => x.WithdrawalType)
            .IsInEnum()
            .WithMessage("Invalid withdrawal type");

        // Validate withdrawal reason if provided
        RuleFor(x => x.Reason)
            .MaximumLength(500)
            .WithMessage("Reason cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Reason));

        // Validate early withdrawal penalty if applicable
        When(x => x.WithdrawalType == WithdrawalType.Early, () =>
        {
            RuleFor(x => x.PenaltyAmount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Penalty amount cannot be negative");

            RuleFor(x => x.PenaltyPercentage)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Penalty percentage cannot be negative")
                .LessThanOrEqualTo(100)
                .WithMessage("Penalty percentage cannot exceed 100%");
        });

        // Validate maturity withdrawal
        When(x => x.WithdrawalType == WithdrawalType.Maturity, () =>
        {
            RuleFor(x => x.PenaltyAmount)
                .Equal(0)
                .WithMessage("No penalty should apply for maturity withdrawal");
        });
    }
}
