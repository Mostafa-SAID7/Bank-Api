using Bank.Application.DTOs;
using MediatR;

namespace Bank.Application.Commands.Deposit;

/// <summary>
/// Command to create a new fixed deposit account
/// </summary>
public sealed record CreateFixedDepositCommand : IRequest<FixedDepositDto>
{
    public Guid CustomerId { get; init; }
    public Guid DepositProductId { get; init; }
    public Guid LinkedAccountId { get; init; }
    public decimal PrincipalAmount { get; init; }
    public int? TermDays { get; init; }
    public Domain.Enums.MaturityAction? MaturityAction { get; init; }
    public bool? AutoRenewalEnabled { get; init; }
    public int? RenewalTermDays { get; init; }
}
