using Bank.Domain.Enums;
using MediatR;

namespace Bank.Application.Commands.Deposit;

/// <summary>
/// Command to process maturity of a fixed deposit
/// </summary>
public sealed record ProcessMaturityCommand : IRequest<bool>
{
    public Guid DepositId { get; init; }
    public MaturityAction MaturityAction { get; init; }
    public Guid ProcessedByUserId { get; init; }
}
