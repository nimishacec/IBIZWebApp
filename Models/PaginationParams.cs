
namespace IBIZWebApp.Models;


public class PaginationParams
{
    private int _pageNumber = 1;
    private int _pageSize = 10;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 10 : value;
    }

    public string? SearchQuery { get; set; }
    public string? SortBy { get; set; }
    public bool IsDescending { get; set; }

    /// <summary>
    /// Calculate the number of records to skip
    /// </summary>
    public int SkipCount => (PageNumber - 1) * PageSize;
}

/// <summary>
/// Generic paginated response
/// </summary>
public class PaginatedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalCount / PageSize);
    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
}
