public class WeightHistoryDto
{
    public Guid Id { get; set; }
    public Guid RatId { get; set; }
    public string RatName { get; set; } = string.Empty;
    public string RatCode { get; set; } = string.Empty;

    public decimal Weight { get; set; }

    public DateTime RecordedDate { get; set; }

    public string? Note { get; set; }
}