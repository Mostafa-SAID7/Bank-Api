namespace Bank.CoreBanking.Domain.Entities;

/// <summary>
/// Ledger entry for double-entry bookkeeping
/// Every posting creates two entries: one debit, one credit
/// Immutable after creation - provides audit trail
/// </summary>
public class Ledger
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TransactionId { get; set; }
    public Guid AccountId { get; set; }
    
    // Double-entry: debit or credit amount (one is positive, one is negative)
    public decimal Amount { get; set; }
    
    // Classification
    public string LedgerType { get; set; } = string.Empty; // Asset, Liability, Equity, Revenue, Expense
    public string Reference { get; set; } = string.Empty; // Transaction reference or batch identifier
    
    // Audit
    public string Description { get; set; } = string.Empty;
    public DateTime PostedAtUtc { get; set; } = DateTime.UtcNow;
    
    // Immutable by design
    private Ledger() { }

    public static Ledger CreateDebit(
        Guid transactionId,
        Guid accountId,
        decimal amount,
        string ledgerType,
        string reference,
        string description)
    {
        return new Ledger
        {
            Id = Guid.NewGuid(),
            TransactionId = transactionId,
            AccountId = accountId,
            Amount = amount, // Positive for debit
            LedgerType = ledgerType,
            Reference = reference,
            Description = description,
            PostedAtUtc = DateTime.UtcNow
        };
    }

    public static Ledger CreateCredit(
        Guid transactionId,
        Guid accountId,
        decimal amount,
        string ledgerType,
        string reference,
        string description)
    {
        return new Ledger
        {
            Id = Guid.NewGuid(),
            TransactionId = transactionId,
            AccountId = accountId,
            Amount = -amount, // Negative for credit
            LedgerType = ledgerType,
            Reference = reference,
            Description = description,
            PostedAtUtc = DateTime.UtcNow
        };
    }
}
