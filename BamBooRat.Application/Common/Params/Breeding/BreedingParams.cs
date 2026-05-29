public class BreedingParams : PaginationParams
{
    public string? Keyword { get; set; }

    public BreedingStatus? Status { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

}