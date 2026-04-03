using Bank.Domain.Enums;

namespace Bank.Application.Helpers.Statement;

/// <summary>
/// Helper for filtering statement transactions
/// </summary>
public static class StatementFilteringHelper
{
    /// <summary>
    /// Filters transactions by date range
    /// </summary>
    /// <param name="transactions">Transactions to filter</param>
    /// <param name="startDate">Start date (inclusive)</param>
    /// <param name="endDate">End date (inclusive)</param>
    /// <returns>Filtered transactions</returns>
    public static IEnumerable<T> FilterByDateRange<T>(
        IEnumerable<T> transactions,
        DateTime startDate,
        DateTime endDate,
        Func<T, DateTime> dateSelector)
    {
        return transactions.Where(t =>
        {
            var transactionDate = dateSelector(t);
            return transactionDate >= startDate && transactionDate <= endDate;
        });
    }

    /// <summary>
    /// Filters transactions by type
    /// </summary>
    /// <param name="transactions">Transactions to filter</param>
    /// <param name="transactionTypes">Transaction types to include</param>
    /// <returns>Filtered transactions</returns>
    public static IEnumerable<T> FilterByTransactionType<T>(
        IEnumerable<T> transactions,
        IEnumerable<TransactionType> transactionTypes,
        Func<T, TransactionType> typeSelector)
    {
        var typeSet = new HashSet<TransactionType>(transactionTypes);
        return transactions.Where(t => typeSet.Contains(typeSelector(t)));
    }

    /// <summary>
    /// Filters transactions by amount range
    /// </summary>
    /// <param name="transactions">Transactions to filter</param>
    /// <param name="minAmount">Minimum amount (inclusive)</param>
    /// <param name="maxAmount">Maximum amount (inclusive)</param>
    /// <returns>Filtered transactions</returns>
    public static IEnumerable<T> FilterByAmountRange<T>(
        IEnumerable<T> transactions,
        decimal? minAmount,
        decimal? maxAmount,
        Func<T, decimal> amountSelector)
    {
        return transactions.Where(t =>
        {
            var amount = Math.Abs(amountSelector(t));
            if (minAmount.HasValue && amount < minAmount.Value)
                return false;
            if (maxAmount.HasValue && amount > maxAmount.Value)
                return false;
            return true;
        });
    }

    /// <summary>
    /// Filters transactions by description (case-insensitive contains)
    /// </summary>
    /// <param name="transactions">Transactions to filter</param>
    /// <param name="searchText">Text to search for</param>
    /// <returns>Filtered transactions</returns>
    public static IEnumerable<T> FilterByDescription<T>(
        IEnumerable<T> transactions,
        string searchText,
        Func<T, string> descriptionSelector)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return transactions;

        var lowerSearchText = searchText.ToLower();
        return transactions.Where(t =>
        {
            var description = descriptionSelector(t) ?? string.Empty;
            return description.ToLower().Contains(lowerSearchText);
        });
    }

    /// <summary>
    /// Filters transactions by category
    /// </summary>
    /// <param name="transactions">Transactions to filter</param>
    /// <param name="categories">Categories to include</param>
    /// <returns>Filtered transactions</returns>
    public static IEnumerable<T> FilterByCategory<T>(
        IEnumerable<T> transactions,
        IEnumerable<string> categories,
        Func<T, string> categorySelector)
    {
        var categorySet = new HashSet<string>(categories, StringComparer.OrdinalIgnoreCase);
        return transactions.Where(t =>
        {
            var category = categorySelector(t) ?? string.Empty;
            return categorySet.Contains(category);
        });
    }

    /// <summary>
    /// Filters transactions by status
    /// </summary>
    /// <param name="transactions">Transactions to filter</param>
    /// <param name="statuses">Statuses to include</param>
    /// <returns>Filtered transactions</returns>
    public static IEnumerable<T> FilterByStatus<T>(
        IEnumerable<T> transactions,
        IEnumerable<string> statuses,
        Func<T, string> statusSelector)
    {
        var statusSet = new HashSet<string>(statuses, StringComparer.OrdinalIgnoreCase);
        return transactions.Where(t =>
        {
            var status = statusSelector(t) ?? string.Empty;
            return statusSet.Contains(status);
        });
    }

    /// <summary>
    /// Applies multiple filters to transactions
    /// </summary>
    /// <param name="transactions">Transactions to filter</param>
    /// <param name="startDate">Start date filter</param>
    /// <param name="endDate">End date filter</param>
    /// <param name="minAmount">Minimum amount filter</param>
    /// <param name="maxAmount">Maximum amount filter</param>
    /// <param name="searchText">Description search text</param>
    /// <returns>Filtered transactions</returns>
    public static IEnumerable<T> ApplyMultipleFilters<T>(
        IEnumerable<T> transactions,
        DateTime? startDate,
        DateTime? endDate,
        decimal? minAmount,
        decimal? maxAmount,
        string? searchText,
        Func<T, DateTime> dateSelector,
        Func<T, decimal> amountSelector,
        Func<T, string> descriptionSelector)
    {
        var result = transactions;

        if (startDate.HasValue && endDate.HasValue)
            result = FilterByDateRange(result, startDate.Value, endDate.Value, dateSelector);

        if (minAmount.HasValue || maxAmount.HasValue)
            result = FilterByAmountRange(result, minAmount, maxAmount, amountSelector);

        if (!string.IsNullOrWhiteSpace(searchText))
            result = FilterByDescription(result, searchText, descriptionSelector);

        return result;
    }
}
