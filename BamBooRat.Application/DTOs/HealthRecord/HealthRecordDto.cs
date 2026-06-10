public class HealthRecordDto
{
    public Guid Id { get; set; }
    public Guid RatId { get; set; }
    public DateTime RecordDate { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string? Treatment { get; set; }
    public string? Medicine { get; set; }
    public string RatCode { get; set; } = string.Empty;
    public string RatName { get; set; } = string.Empty;
}