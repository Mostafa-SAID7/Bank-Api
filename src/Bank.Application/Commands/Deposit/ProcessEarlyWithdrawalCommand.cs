using Bank.Application.DTOs;
using MediatR;

namespace Bank.Application.Commands.Deposit;

/// <summary>
/// Command to process early withdrawal from a fixed deposit
/// </summary>
public sealed record ProcessEarlyWithdrawalCommand : IRequest<WithdrawalResult>
{
    public Guid DepositId { get; init; }
    public decimal WithdrawalAmount { get; init; }
    public string? Reason { get; init; }
    public bool AcknowledgePenalty { get; init; }
    public Guid ProcessedByUserId { get; init; }
}
