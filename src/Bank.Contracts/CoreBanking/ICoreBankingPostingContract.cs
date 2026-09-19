namespace Bank.Contracts.CoreBanking;

public interface ICoreBankingPostingContract
{
    Task<PostingResult> PostPaymentAsync(
        PostPaymentCommand command,
        CancellationToken cancellationToken = default);
}

public sealed record PostPaymentCommand(
    Guid PaymentId,
    Guid AccountId,
    decimal Amount,
    string Currency,
    string IdempotencyKey);

public sealed record PostingResult(
    bool Succeeded,
    string? Reference,
    string? FailureReason = null);