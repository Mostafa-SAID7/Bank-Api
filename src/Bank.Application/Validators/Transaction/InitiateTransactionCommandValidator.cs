using FluentValidation;
using Bank.Application.Commands.Transaction;

namespace Bank.Application.Validators.Transaction;

/// <summary>
/// Validator for initiate transaction commands
/// </summary>
public class InitiateTransactionCommandValidator : AbstractValidator<InitiateTransactionCommand>
{
    public InitiateTransactionCommandValidator()
    {
        RuleFor(x => x.FromAccountId)
            .NotEmpty().WithMessage("Source account ID is required.");
            
        RuleFor(x => x.ToAccountId)
            .NotEmpty().WithMessage("Destination account ID is required.")
            .NotEqual(x => x.FromAccountId).WithMessage("Source and destination accounts must be different.");
            
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Transfer amount must be greater than zero.");
            
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
            
        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid transaction type.");
    }
}
