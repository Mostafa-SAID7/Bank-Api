namespace Bank.CoreBanking.Presentation.Dtos;

/// <summary>
/// Response DTO for account details
/// </summary>
public sealed record AccountResponse(
    Guid Id,
    string AccountNumber,
    string AccountHolderName,
    Guid CustomerId,
    decimal Balance,
    string Currency,
    int Status,
    int Type,
    DateTime OpenedAtUtc,
    DateTime LastActivityDate
);
