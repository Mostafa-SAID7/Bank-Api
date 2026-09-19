namespace Bank.CoreBanking.Presentation.Dtos;

/// <summary>
/// Request DTO for creating a new bank account
/// </summary>
public sealed record CreateAccountRequest(
    string AccountHolderName,
    Guid CustomerId,
    string Currency = "USD",
    int Type = 0) // 0 = Checking, 1 = Savings
{
    public CreateAccountRequest() : this("", Guid.Empty) { }
}
