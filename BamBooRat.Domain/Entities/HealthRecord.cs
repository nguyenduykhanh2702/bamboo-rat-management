public class HealthRecord : BaseEntity
{
    public Guid RatId { get; set; }
    public Rat Rat { get; set; } = null!;
    public DateTime RecordDate { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public string? Medicine { get; set; }
    public string? Note { get; set; }
}