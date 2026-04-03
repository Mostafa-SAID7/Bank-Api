using Bank.Application.DTOs.Account.JointAccount;
using Bank.Domain.Enums;
using FluentValidation;

namespace Bank.Application.Validators.Account;

/// <summary>
/// Validator for joint account requests
/// </summary>
public class JointAccountRequestValidator : AbstractValidator<AddJointHolderRequest>
{
    public JointAccountRequestValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("Account ID is required");

        RuleFor(x => x.JointHolderId)
            .NotEmpty()
            .WithMessage("Joint holder ID is required");

        RuleFor(x => x)
            .Must(x => x.AccountId != x.JointHolderId)
            .WithMessage("Joint holder cannot be the same as the account owner")
            .WithName("JointHolderValidation");

        RuleFor(x => x.JointHolderName)
            .NotEmpty()
            .WithMessage("Joint holder name is required")
            .MaximumLength(200)
            .WithMessage("Joint holder name cannot exceed 200 characters");

        RuleFor(x => x.JointHolderEmail)
            .NotEmpty()
            .WithMessage("Joint holder email is required")
            .EmailAddress()
            .WithMessage("Invalid email address format");

        RuleFor(x => x.JointHolderPhone)
            .NotEmpty()
            .WithMessage("Joint holder phone is required")
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Invalid phone number format");

        RuleFor(x => x.AccessLevel)
            .IsInEnum()
            .WithMessage("Invalid access level");

        // Validate access level specific rules
        When(x => x.AccessLevel == JointAccountAccessLevel.ReadOnly, () =>
        {
            RuleFor(x => x.CanInitiateTransfers)
                .Equal(false)
                .WithMessage("Read-only access cannot initiate transfers");

            RuleFor(x => x.CanApproveTransfers)
                .Equal(false)
                .WithMessage("Read-only access cannot approve transfers");
        });

        When(x => x.AccessLevel == JointAccountAccessLevel.Limited, () =>
        {
            RuleFor(x => x.TransactionLimit)
                .GreaterThan(0)
                .WithMessage("Limited access requires a transaction limit")
                .When(x => x.TransactionLimit.HasValue);
        });

        // Validate permissions
        RuleFor(x => x.CanInitiateTransfers)
            .NotNull()
            .WithMessage("Transfer initiation permission is required");

        RuleFor(x => x.CanApproveTransfers)
            .NotNull()
            .WithMessage("Transfer approval permission is required");

        // Validate that approval requires initiation
        RuleFor(x => x)
            .Must(x => !x.CanApproveTransfers.Value || x.CanInitiateTransfers.Value)
            .WithMessage("Cannot approve transfers without initiating transfers")
            .WithName("PermissionValidation");

        // Validate effective date if provided
        RuleFor(x => x.EffectiveDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Effective date cannot be in the future")
            .When(x => x.EffectiveDate.HasValue);
    }
}
