public class WeightHistory : BaseEntity
{
    public Guid RatId { get; set; }
    public Rat Rat { get; set; } = null!;
    public decimal Weight { get; set; }
    public DateTime RecordedDate { get; set; }
    public string? Note { get; set; }
}