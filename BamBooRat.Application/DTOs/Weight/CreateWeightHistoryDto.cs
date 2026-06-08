public class CreateWeightHistoryDto
{
    public Guid RatId { get; set; }

    public decimal Weight { get; set; }

    public DateTime RecordedDate { get; set; }

    public string? Note { get; set; }
}