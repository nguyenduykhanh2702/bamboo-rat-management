public class CageTransferParams : PaginationParams
{
    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string FromCageName { get; set; } = string.Empty;
    public string ToCageName { get; set; } = string.Empty;
    public string RatName { get; set; } = string.Empty;
    public string RatCode { get; set; } = string.Empty;
}