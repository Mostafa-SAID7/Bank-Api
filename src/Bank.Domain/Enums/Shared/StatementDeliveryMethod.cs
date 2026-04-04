namespace Bank.Domain.Enums;

/// <summary>
/// Statement delivery methods
/// </summary>
public enum StatementDeliveryMethod
{
    Email = 1,
    Download = 2,
    PostalMail = 3, // Alias for Mail
    SMS = 4,
    API = 5
}
