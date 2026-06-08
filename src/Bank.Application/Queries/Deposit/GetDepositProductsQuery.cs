using Bank.Application.DTOs;
using Bank.Domain.Enums;
using MediatR;

namespace Bank.Application.Queries.Deposit;

/// <summary>
/// Query to retrieve deposit products
/// </summary>
public sealed record GetDepositProductsQuery : IRequest<IEnumerable<DepositProductDto>>
{
    public DepositProductQueryType QueryType { get; init; }
    public DepositProductType? ProductType { get; init; }
    public Guid? ProductId { get; init; }
}

public enum DepositProductQueryType
{
    All,
    Active,
    ByType,
    ById
}
