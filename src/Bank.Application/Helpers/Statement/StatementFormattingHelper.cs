using System.Globalization;
using Bank.Domain.Enums;

namespace Bank.Application.Helpers.Statement;

/// <summary>
/// Helper for formatting statement data
/// </summary>
public static class StatementFormattingHelper
{
    private static readonly CultureInfo UsCulture = new("en-US");

    /// <summary>
    /// Formats currency amount with proper formatting
    /// </summary>
    /// <param name="amount">Amount to format</param>
    /// <param name="currencyCode">Currency code (default: USD)</param>
    /// <returns>Formatted currency string</returns>
    public static string FormatCurrency(decimal amount, string currencyCode = "USD")
    {
        return amount.ToString("C", UsCulture);
    }

    /// <summary>
    /// Formats date for statement display
    /// </summary>
    /// <param name="date">Date to format</param>
    /// <returns>Formatted date string (MM/dd/yyyy)</returns>
    public static string FormatDate(DateTime date)
    {
        return date.ToString("MM/dd/yyyy", UsCulture);
    }

    /// <summary>
    /// Formats date and time for statement display
    /// </summary>
    /// <param name="dateTime">DateTime to format</param>
    /// <returns>Formatted datetime string (MM/dd/yyyy HH:mm:ss)</returns>
    public static string FormatDateTime(DateTime dateTime)
    {
        return dateTime.ToString("MM/dd/yyyy HH:mm:ss", UsCulture);
    }

    /// <summary>
    /// Formats transaction description for display
    /// </summary>
    /// <param name="description">Original description</param>
    /// <param name="maxLength">Maximum length (default: 50)</param>
    /// <returns>Formatted description</returns>
    public static string FormatTransactionDescription(string description, int maxLength = 50)
    {
        if (string.IsNullOrEmpty(description))
            return "N/A";

        if (description.Length <= maxLength)
            return description;

        return description.Substring(0, maxLength - 3) + "...";
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
    /// Formats account number for display (masked)
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
    /// Formats balance for display with sign indicator
    /// </summary>
    /// <param name="balance">Balance amount</param>
    /// <returns>Formatted balance with sign</returns>
    public static string FormatBalance(decimal balance)
    {
        var formatted = FormatCurrency(balance);
        return balance < 0 ? $"({formatted})" : formatted;
    }

    /// <summary>
    /// Formats statement period for display
    /// </summary>
    /// <param name="startDate">Period start date</param>
    /// <param name="endDate">Period end date</param>
    /// <returns>Formatted period string (e.g., "01/01/2024 - 01/31/2024")</returns>
    public static string FormatStatementPeriod(DateTime startDate, DateTime endDate)
    {
        return $"{FormatDate(startDate)} - {FormatDate(endDate)}";
    }

    /// <summary>
    /// Formats statement status for display
    /// </summary>
    /// <param name="status">Statement status enum</param>
    /// <returns>Formatted status string</returns>
    public static string FormatStatementStatus(StatementStatus status)
    {
        return status switch
        {
            StatementStatus.Draft => "Draft",
            StatementStatus.Generated => "Generated",
            StatementStatus.Delivered => "Delivered",
            StatementStatus.Viewed => "Viewed",
            StatementStatus.Archived => "Archived",
            _ => "Unknown"
        };
    }

    /// <summary>
    /// Formats statement format for display
    /// </summary>
    /// <param name="format">Statement format enum</param>
    /// <returns>Formatted format string</returns>
    public static string FormatStatementFormat(StatementFormat format)
    {
        return format switch
        {
            StatementFormat.PDF => "PDF",
            StatementFormat.CSV => "CSV",
            StatementFormat.Excel => "Excel",
            StatementFormat.HTML => "HTML",
            _ => "Unknown"
        };
    }
}
