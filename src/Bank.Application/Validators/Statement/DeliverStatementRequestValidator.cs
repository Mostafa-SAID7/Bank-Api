using Bank.Application.DTOs.Statement.Delivery;
using Bank.Domain.Enums;
using FluentValidation;

namespace Bank.Application.Validators.Statement;

/// <summary>
/// Validator for deliver statement requests
/// </summary>
public class DeliverStatementRequestValidator : AbstractValidator<DeliverStatementRequest>
{
    public DeliverStatementRequestValidator()
    {
        RuleFor(x => x.DeliveryMethod)
            .IsInEnum()
            .WithMessage("Invalid delivery method");

        RuleFor(x => x.DeliveryAddress)
            .NotEmpty()
            .WithMessage("Delivery address is required")
            .MaximumLength(500)
            .WithMessage("Delivery address cannot exceed 500 characters");

        // Validate delivery address format based on delivery method
        When(x => x.DeliveryMethod == StatementDeliveryMethod.Email, () =>
        {
            RuleFor(x => x.DeliveryAddress)
                .EmailAddress()
                .WithMessage("Invalid email address format for email delivery");
        });

        When(x => x.DeliveryMethod == StatementDeliveryMethod.SMS, () =>
        {
            RuleFor(x => x.DeliveryAddress)
                .Matches(@"^\+?[1-9]\d{1,14}$")
                .WithMessage("Invalid phone number format for SMS delivery");
        });

        When(x => x.DeliveryMethod == StatementDeliveryMethod.PostalMail, () =>
        {
            RuleFor(x => x.DeliveryAddress)
                .MinimumLength(10)
                .WithMessage("Postal address must be at least 10 characters")
                .MaximumLength(500)
                .WithMessage("Postal address cannot exceed 500 characters");
        });
    }
}
