using Bank.Application.DTOs;
using MediatR;

namespace Bank.Application.Queries.Deposit;

/// <summary>
/// Query to retrieve all deposits for a customer
/// </summary>
public sealed record GetCustomerDepositsQuery : IRequest<IEnumerable<FixedDepositDto>>
{
    public Guid CustomerId { get; init; }
}
