using Bank.Application.DTOs.Account.Core;
using Bank.Domain.Enums;
using FluentValidation;

namespace Bank.Application.Validators.Account;

/// <summary>
/// Validator for update account requests
/// </summary>
public class UpdateAccountRequestValidator : AbstractValidator<UpdateAccountRequest>
{
    public UpdateAccountRequestValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("Account ID is required");

        // Validate account nickname if provided
        RuleFor(x => x.AccountNickname)
            .MaximumLength(100)
            .WithMessage("Account nickname cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.AccountNickname));

        // Validate status if provided
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid account status")
            .When(x => x.Status.HasValue);

        // Validate transfer limits if provided
        When(x => x.DailyTransferLimit.HasValue || x.MonthlyTransferLimit.HasValue, () =>
        {
            RuleFor(x => x.DailyTransferLimit)
                .GreaterThan(0)
                .WithMessage("Daily transfer limit must be greater than zero")
                .When(x => x.DailyTransferLimit.HasValue);

            RuleFor(x => x.MonthlyTransferLimit)
                .GreaterThan(0)
                .WithMessage("Monthly transfer limit must be greater than zero")
                .When(x => x.MonthlyTransferLimit.HasValue);

            RuleFor(x => x)
                .Must(x => !x.DailyTransferLimit.HasValue || !x.MonthlyTransferLimit.HasValue || 
                           x.DailyTransferLimit <= x.MonthlyTransferLimit)
                .WithMessage("Daily transfer limit cannot exceed monthly transfer limit")
                .WithName("TransferLimits");
        });

        // Validate overdraft protection if provided
        RuleFor(x => x.OverdraftProtectionEnabled)
            .NotNull()
            .WithMessage("Overdraft protection setting is required")
            .When(x => x.OverdraftProtectionEnabled.HasValue);

        // Validate overdraft limit if overdraft protection is enabled
        When(x => x.OverdraftProtectionEnabled == true, () =>
        {
            RuleFor(x => x.OverdraftLimit)
                .GreaterThan(0)
                .WithMessage("Overdraft limit must be greater than zero")
                .When(x => x.OverdraftLimit.HasValue);
        });

        // Validate interest rate if provided
        RuleFor(x => x.InterestRate)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Interest rate cannot be negative")
            .LessThanOrEqualTo(100)
            .WithMessage("Interest rate cannot exceed 100%")
            .When(x => x.InterestRate.HasValue);

        // Validate notification preferences if provided
        RuleFor(x => x.NotificationPreferences)
            .NotEmpty()
            .WithMessage("Notification preferences cannot be empty")
            .When(x => x.NotificationPreferences != null && x.NotificationPreferences.Any());
    }
}
