namespace Bank.Application.DTOs.Card.Advanced;

/// <summary>
/// Base class for paged requests
/// </summary>
public class PagedRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = true;
}




