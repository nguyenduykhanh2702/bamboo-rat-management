public class PaginationParams
{
    private const int MaxPageSize = 50;
    private int _pageNumber = 1;
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value <= 0 ? 1 : value;
    }

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set
        {
            if (value <= 0)
                _pageSize = 10;
            else
                _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
    public string? Search { get; set; }

    // Sort
    public string? OrderBy { get; set; } // "name", "name desc"

}