using System.Globalization;
using Bank.Domain.Enums;

namespace Bank.Application.Helpers.Transaction;

/// <summary>
/// Helper for formatting transaction data
/// </summary>
public static class TransactionFormattingHelper
{
    private static readonly CultureInfo UsCulture = new("en-US");

    /// <summary>
    /// Formats currency amount
    /// </summary>
    /// <param name="amount">Amount to format</param>
    /// <returns>Formatted currency string</returns>
    public static string FormatCurrency(decimal amount)
    {
        return amount.ToString("C", UsCulture);
    }

    /// <summary>
    /// Formats amount with sign indicator
    /// </summary>
    /// <param name="amount">Amount to format</param>
    /// <param name="transactionType">Transaction type</param>
    /// <returns>Formatted amount with sign</returns>
    public static string FormatAmountWithSign(decimal amount, TransactionType transactionType)
    {
        var isDebit = IsDebitTransaction(transactionType);
        var sign = isDebit ? "-" : "+";
        return $"{sign}{FormatCurrency(Math.Abs(amount))}";
    }

    /// <summary>
    /// Formats transaction date
    /// </summary>
    /// <param name="date">Date to format</param>
    /// <returns>Formatted date string (MM/dd/yyyy)</returns>
    public static string FormatDate(DateTime date)
    {
        return date.ToString("MM/dd/yyyy", UsCulture);
    }

    /// <summary>
    /// Formats transaction datetime
    /// </summary>
    /// <param name="dateTime">DateTime to format</param>
    /// <returns>Formatted datetime string (MM/dd/yyyy HH:mm:ss)</returns>
    public static string FormatDateTime(DateTime dateTime)
    {
        return dateTime.ToString("MM/dd/yyyy HH:mm:ss", UsCulture);
    }

    /// <summary>
    /// Formats transaction type for display
    /// </summary>
    /// <param name="transactionType">Transaction type enum</param>
    /// <returns>Formatted transaction type string</returns>
    public static string FormatTransactionType(TransactionType transactionType)
    {
        return transactionType switch
        {
            TransactionType.Deposit => "Deposit",
            TransactionType.Withdrawal => "Withdrawal",
            TransactionType.Transfer => "Transfer",
            TransactionType.Payment => "Payment",
            TransactionType.Fee => "Fee",
            TransactionType.Interest => "Interest",
            TransactionType.Refund => "Refund",
            TransactionType.Reversal => "Reversal",
            _ => "Other"
        };
    }

    /// <summary>
    /// Formats transaction status for display
    /// </summary>
    /// <param name="status">Transaction status enum</param>
    /// <returns>Formatted status string</returns>
    public static string FormatStatus(TransactionStatus status)
    {
        return status switch
        {
            TransactionStatus.Pending => "Pending",
            TransactionStatus.Processing => "Processing",
            TransactionStatus.Completed => "Completed",
            TransactionStatus.Failed => "Failed",
            TransactionStatus.Cancelled => "Cancelled",
            TransactionStatus.Settled => "Settled",
            TransactionStatus.Reversed => "Reversed",
            _ => "Unknown"
        };
    }

    /// <summary>
    /// Formats transaction description (truncated)
    /// </summary>
    /// <param name="description">Description to format</param>
    /// <param name="maxLength">Maximum length (default: 50)</param>
    /// <returns>Formatted description</returns>
    public static string FormatDescription(string description, int maxLength = 50)
    {
        if (string.IsNullOrEmpty(description))
            return "N/A";

        if (description.Length <= maxLength)
            return description;

        return description.Substring(0, maxLength - 3) + "...";
    }

    /// <summary>
    /// Formats reference number
    /// </summary>
    /// <param name="reference">Reference to format</param>
    /// <returns>Formatted reference</returns>
    public static string FormatReference(string? reference)
    {
        return string.IsNullOrEmpty(reference) ? "N/A" : reference;
    }

    /// <summary>
    /// Formats account number (masked)
    /// </summary>
    /// <param name="accountNumber">Full account number</param>
    /// <returns>Masked account number (e.g., ****1234)</returns>
    public static string FormatMaskedAccountNumber(string accountNumber)
    {
        if (string.IsNullOrEmpty(accountNumber) || accountNumber.Length < 4)
            return "****";

        var lastFour = accountNumber.Substring(accountNumber.Length - 4);
        return $"****{lastFour}";
    }

    /// <summary>
    /// Formats transaction ID (shortened)
    /// </summary>
    /// <param name="transactionId">Transaction ID</param>
    /// <returns>Shortened transaction ID</returns>
    public static string FormatTransactionId(Guid transactionId)
    {
        return transactionId.ToString().Substring(0, 8).ToUpper();
    }

    /// <summary>
    /// Formats transaction summary
    /// </summary>
    /// <param name="type">Transaction type</param>
    /// <param name="amount">Amount</param>
    /// <param name="description">Description</param>
    /// <returns>Formatted summary</returns>
    public static string FormatTransactionSummary(TransactionType type, decimal amount, string description)
    {
        var typeStr = FormatTransactionType(type);
        var amountStr = FormatCurrency(amount);
        var descStr = FormatDescription(description, 30);
        return $"{typeStr}: {amountStr} - {descStr}";
    }

    /// <summary>
    /// Checks if transaction is a debit
    /// </summary>
    /// <param name="transactionType">Transaction type</param>
    /// <returns>True if debit</returns>
    private static bool IsDebitTransaction(TransactionType transactionType)
    {
        return transactionType is TransactionType.Withdrawal 
                              or TransactionType.Payment 
                              or TransactionType.Fee;
    }

    /// <summary>
    /// Gets transaction icon/symbol
    /// </summary>
    /// <param name="transactionType">Transaction type</param>
    /// <returns>Icon symbol</returns>
    public static string GetTransactionIcon(TransactionType transactionType)
    {
        return transactionType switch
        {
            TransactionType.Deposit => "↓",
            TransactionType.Withdrawal => "↑",
            TransactionType.Transfer => "↔",
            TransactionType.Payment => "→",
            TransactionType.Fee => "⊖",
            TransactionType.Interest => "⊕",
            TransactionType.Refund => "↩",
            TransactionType.Reversal => "⟲",
            _ => "•"
        };
    }
}
