using Bank.Application.DTOs.Account.Core;
using Bank.Domain.Enums;
using FluentValidation;

namespace Bank.Application.Validators.Account;

/// <summary>
/// Validator for create account requests
/// </summary>
public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
{
    public CreateAccountRequestValidator()
    {
        RuleFor(x => x.AccountType)
            .IsInEnum()
            .WithMessage("Invalid account type");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithMessage("Currency is required")
            .Length(3)
            .WithMessage("Currency code must be 3 characters");

        RuleFor(x => x.InitialDeposit)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Initial deposit cannot be negative");

        // Validate minimum initial deposit based on account type
        When(x => x.AccountType == AccountType.Savings, () =>
        {
            RuleFor(x => x.InitialDeposit)
                .GreaterThanOrEqualTo(100)
                .WithMessage("Savings account requires minimum initial deposit of $100");
        });

        When(x => x.AccountType == AccountType.Checking, () =>
        {
            RuleFor(x => x.InitialDeposit)
                .GreaterThanOrEqualTo(50)
                .WithMessage("Checking account requires minimum initial deposit of $50");
        });

        When(x => x.AccountType == AccountType.MoneyMarket, () =>
        {
            RuleFor(x => x.InitialDeposit)
                .GreaterThanOrEqualTo(2500)
                .WithMessage("Money Market account requires minimum initial deposit of $2,500");
        });

        When(x => x.AccountType == AccountType.CertificateOfDeposit, () =>
        {
            RuleFor(x => x.InitialDeposit)
                .GreaterThanOrEqualTo(1000)
                .WithMessage("Certificate of Deposit requires minimum initial deposit of $1,000");
        });

        // Validate account nickname if provided
        RuleFor(x => x.AccountNickname)
            .MaximumLength(100)
            .WithMessage("Account nickname cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.AccountNickname));

        // Validate overdraft protection if applicable
        When(x => x.AccountType == AccountType.Checking, () =>
        {
            RuleFor(x => x.OverdraftProtectionEnabled)
                .NotNull()
                .WithMessage("Overdraft protection setting is required for checking accounts");
        });

        // Validate interest rate if provided
        RuleFor(x => x.InterestRate)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Interest rate cannot be negative")
            .LessThanOrEqualTo(100)
            .WithMessage("Interest rate cannot exceed 100%")
            .When(x => x.InterestRate.HasValue);
    }
}
