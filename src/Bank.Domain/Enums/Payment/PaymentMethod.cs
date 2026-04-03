namespace Bank.Domain.Enums;

/// <summary>
/// Payment methods supported by the system
/// </summary>
public enum PaymentMethod
{
    BankTransfer = 1,
    DebitCard = 2,
    CreditCard = 3,
    DigitalWallet = 4,
    Check = 5,
    Cash = 6,
    ACH = 7,
    WireTransfer = 8
}
