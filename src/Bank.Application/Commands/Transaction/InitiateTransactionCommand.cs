using MediatR;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Application.Interfaces;

namespace Bank.Application.Commands.Transaction;

public record InitiateTransactionCommand(
    Guid FromAccountId, 
    Guid ToAccountId, 
    decimal Amount, 
    TransactionType Type, 
    string Description) : IRequest<Transaction>;

public class InitiateTransactionCommandHandler : IRequestHandler<InitiateTransactionCommand, Transaction>
{
    private readonly ITransactionService _transactionService;

    public InitiateTransactionCommandHandler(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task<Transaction> Handle(InitiateTransactionCommand request, CancellationToken cancellationToken)
    {
        // For CQRS we are delegating to the domain service, but this could be moved 
        // entirely into the handler in a pure CQRS setup.
        return await _transactionService.InitiateTransactionAsync(
            request.FromAccountId, 
            request.ToAccountId, 
            request.Amount, 
            request.Type, 
            request.Description);
    }
}
