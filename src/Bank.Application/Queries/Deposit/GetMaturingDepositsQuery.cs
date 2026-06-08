using Bank.Application.DTOs;
using MediatR;

namespace Bank.Application.Queries.Deposit;

/// <summary>
/// Query to retrieve deposits maturing within a date range
/// </summary>
public sealed record GetMaturingDepositsQuery : IRequest<IEnumerable<FixedDepositDto>>
{
    public DateTime FromDate { get; init; }
    public DateTime ToDate { get; init; }
}
