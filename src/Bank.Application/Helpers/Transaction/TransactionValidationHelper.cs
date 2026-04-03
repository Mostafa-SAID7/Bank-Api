using Bank.Domain.Enums;

namespace Bank.Application.Helpers.Transaction;

/// <summary>
/// Helper for transaction validation
/// </summary>
public static class TransactionValidationHelper
{
    /// <summary>
    /// Validates transaction amount
    /// </summary>
    /// <param name="amount">Amount to validate</param>
    /// <param name="minAmount">Minimum allowed amount</param>
    /// <param name="maxAmount">Maximum allowed amount</param>
    /// <returns>Tuple of (isValid, errorMessage)</returns>
    public static (bool IsValid, string ErrorMessage) ValidateAmount(
        decimal amount,
        decimal minAmount = 0.01m,
        decimal maxAmount = 999_999_999.99m)
    {
        if (amount < minAmount)
            return (false, $"Amount must be at least {minAmount:C}");

        if (amount > maxAmount)
            return (false, $"Amount cannot exceed {maxAmount:C}");

        return (true, string.Empty);
    }

    /// <summary>
    /// Validates transaction type
    /// </summary>
    /// <param name="transactionType">Transaction type to validate</param>
    /// <returns>Tuple of (isValid, errorMessage)</returns>
    public static (bool IsValid, string ErrorMessage) ValidateTransactionType(TransactionType transactionType)
    {
        if (!Enum.IsDefined(typeof(TransactionType), transactionType))
            return (false, "Invalid transaction type");

        return (true, string.Empty);
    }

    /// <summary>
    /// Validates transaction date
    /// </summary>
    /// <param name="transactionDate">Date to validate</param>
    /// <returns>Tuple of (isValid, errorMessage)</returns>
    public static (bool IsValid, string ErrorMessage) ValidateTransactionDate(DateTime transactionDate)
    {
        if (transactionDate > DateTime.UtcNow)
            return (false, "Transaction date cannot be in the future");

        if (transactionDate < DateTime.UtcNow.AddYears(-10))
            return (false, "Transaction date cannot be more than 10 years in the past");

        return (true, string.Empty);
    }

    /// <summary>
    /// Validates account IDs for transfer
    /// </summary>
    /// <param name="fromAccountId">Source account ID</param>
    /// <param name="toAccountId">Destination account ID</param>
    /// <returns>Tuple of (isValid, errorMessage)</returns>
    public static (bool IsValid, string ErrorMessage) ValidateTransferAccounts(Guid fromAccountId, Guid toAccountId)
    {
        if (fromAccountId == Guid.Empty)
            return (false, "Source account ID is required");

        if (toAccountId == Guid.Empty)
            return (false, "Destination account ID is required");

        if (fromAccountId == toAccountId)
            return (false, "Cannot transfer to the same account");

        return (true, string.Empty);
    }

    /// <summary>
    /// Validates transaction description
    /// </summary>
    /// <param name="description">Description to validate</param>
    /// <param name="maxLength">Maximum length (default: 500)</param>
    /// <returns>Tuple of (isValid, errorMessage)</returns>
    public static (bool IsValid, string ErrorMessage) ValidateDescription(string description, int maxLength = 500)
    {
        if (string.IsNullOrWhiteSpace(description))
            return (false, "Description is required");

        if (description.Length > maxLength)
            return (false, $"Description cannot exceed {maxLength} characters");

        return (true, string.Empty);
    }

    /// <summary>
    /// Validates transaction reference
    /// </summary>
    /// <param name="reference">Reference to validate</param>
    /// <param name="maxLength">Maximum length (default: 100)</param>
    /// <returns>Tuple of (isValid, errorMessage)</returns>
    public static (bool IsValid, string ErrorMessage) ValidateReference(string? reference, int maxLength = 100)
    {
        if (string.IsNullOrEmpty(reference))
            return (true, string.Empty); // Reference is optional

        if (reference.Length > maxLength)
            return (false, $"Reference cannot exceed {maxLength} characters");

        return (true, string.Empty);
    }

    /// <summary>
    /// Validates transaction status
    /// </summary>
    /// <param name="status">Status to validate</param>
    /// <returns>Tuple of (isValid, errorMessage)</returns>
    public static (bool IsValid, string ErrorMessage) ValidateStatus(TransactionStatus status)
    {
        if (!Enum.IsDefined(typeof(TransactionStatus), status))
            return (false, "Invalid transaction status");

        return (true, string.Empty);
    }

    /// <summary>
    /// Checks if transaction can be cancelled
    /// </summary>
    /// <param name="status">Current transaction status</param>
    /// <returns>True if can be cancelled</returns>
    public static bool CanBeCancelled(TransactionStatus status)
    {
        return status is TransactionStatus.Pending or TransactionStatus.Processing;
    }

    /// <summary>
    /// Checks if transaction can be refunded
    /// </summary>
    /// <param name="status">Current transaction status</param>
    /// <returns>True if can be refunded</returns>
    public static bool CanBeRefunded(TransactionStatus status)
    {
        return status is TransactionStatus.Completed or TransactionStatus.Settled;
    }

    /// <summary>
    /// Checks if transaction is in final state
    /// </summary>
    /// <param name="status">Transaction status</param>
    /// <returns>True if in final state</returns>
    public static bool IsInFinalState(TransactionStatus status)
    {
        return status is TransactionStatus.Completed 
                     or TransactionStatus.Failed 
                     or TransactionStatus.Cancelled 
                     or TransactionStatus.Settled;
    }
}
