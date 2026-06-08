using Bank.Application.DTOs;
using MediatR;

namespace Bank.Application.Commands.Deposit;

/// <summary>
/// Command to renew a fixed deposit at maturity
/// </summary>
public sealed record RenewFixedDepositCommand : IRequest<FixedDepositDto>
{
    public Guid DepositId { get; init; }
    public int TermDays { get; init; }
    public bool? AutoRenewalEnabled { get; init; }
    public Guid ProcessedByUserId { get; init; }
}
