namespace Bank.Domain.Enums;

/// <summary>
/// Loan document types
/// </summary>
public enum LoanDocumentType
{
    Application = 1,
    Agreement = 2,
    IncomeProof = 3,
    IdentityProof = 4,
    AddressProof = 5,
    Collateral = 6,
    Insurance = 7,
    Other = 8
}
