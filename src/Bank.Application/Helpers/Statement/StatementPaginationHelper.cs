namespace Bank.Application.Helpers.Statement;

/// <summary>
/// Helper for statement pagination operations
/// </summary>
public static class StatementPaginationHelper
{
    /// <summary>
    /// Calculates the skip count for pagination
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>Number of items to skip</returns>
    public static int CalculateSkip(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            pageNumber = 1;
        if (pageSize < 1)
            pageSize = 20;

        return (pageNumber - 1) * pageSize;
    }

    /// <summary>
    /// Validates pagination parameters
    /// </summary>
    /// <param name="pageNumber">Page number to validate</param>
    /// <param name="pageSize">Page size to validate</param>
    /// <returns>Tuple of (isValid, errorMessage)</returns>
    public static (bool IsValid, string ErrorMessage) ValidatePaginationParameters(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            return (false, "Page number must be greater than 0");

        if (pageSize < 1)
            return (false, "Page size must be greater than 0");

        if (pageSize > 100)
            return (false, "Page size cannot exceed 100");

        return (true, string.Empty);
    }

    /// <summary>
    /// Calculates total pages
    /// </summary>
    /// <param name="totalItems">Total number of items</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>Total number of pages</returns>
    public static int CalculateTotalPages(int totalItems, int pageSize)
    {
        if (pageSize < 1)
            pageSize = 20;

        return (int)Math.Ceiling((double)totalItems / pageSize);
    }

    /// <summary>
    /// Determines if there is a next page
    /// </summary>
    /// <param name="pageNumber">Current page number</param>
    /// <param name="totalPages">Total number of pages</param>
    /// <returns>True if there is a next page</returns>
    public static bool HasNextPage(int pageNumber, int totalPages)
    {
        return pageNumber < totalPages;
    }

    /// <summary>
    /// Determines if there is a previous page
    /// </summary>
    /// <param name="pageNumber">Current page number</param>
    /// <returns>True if there is a previous page</returns>
    public static bool HasPreviousPage(int pageNumber)
    {
        return pageNumber > 1;
    }

    /// <summary>
    /// Gets the next page number
    /// </summary>
    /// <param name="pageNumber">Current page number</param>
    /// <param name="totalPages">Total number of pages</param>
    /// <returns>Next page number or current page if at end</returns>
    public static int GetNextPageNumber(int pageNumber, int totalPages)
    {
        return HasNextPage(pageNumber, totalPages) ? pageNumber + 1 : pageNumber;
    }

    /// <summary>
    /// Gets the previous page number
    /// </summary>
    /// <param name="pageNumber">Current page number</param>
    /// <returns>Previous page number or 1 if at start</returns>
    public static int GetPreviousPageNumber(int pageNumber)
    {
        return HasPreviousPage(pageNumber) ? pageNumber - 1 : 1;
    }

    /// <summary>
    /// Paginates a collection
    /// </summary>
    /// <param name="items">Items to paginate</param>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>Paginated items</returns>
    public static IEnumerable<T> Paginate<T>(IEnumerable<T> items, int pageNumber, int pageSize)
    {
        var skip = CalculateSkip(pageNumber, pageSize);
        return items.Skip(skip).Take(pageSize);
    }

    /// <summary>
    /// Creates pagination metadata
    /// </summary>
    /// <param name="pageNumber">Current page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="totalItems">Total number of items</param>
    /// <returns>Pagination metadata</returns>
    public static PaginationMetadata CreatePaginationMetadata(int pageNumber, int pageSize, int totalItems)
    {
        var totalPages = CalculateTotalPages(totalItems, pageSize);

        return new PaginationMetadata
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            HasNextPage = HasNextPage(pageNumber, totalPages),
            HasPreviousPage = HasPreviousPage(pageNumber),
            NextPageNumber = GetNextPageNumber(pageNumber, totalPages),
            PreviousPageNumber = GetPreviousPageNumber(pageNumber)
        };
    }
}

/// <summary>
/// Pagination metadata
/// </summary>
public class PaginationMetadata
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
    public int NextPageNumber { get; set; }
    public int PreviousPageNumber { get; set; }
}
