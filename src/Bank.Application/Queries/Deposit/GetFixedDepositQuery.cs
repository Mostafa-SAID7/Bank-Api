using Bank.Application.DTOs;
using MediatR;

namespace Bank.Application.Queries.Deposit;

/// <summary>
/// Query to retrieve a fixed deposit by ID
/// </summary>
public sealed record GetFixedDepositQuery : IRequest<FixedDepositDto?>
{
    public Guid DepositId { get; init; }
}
