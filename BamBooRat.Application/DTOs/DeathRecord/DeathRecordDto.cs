public class DeathRecordDto
{
    public Guid Id { get; set; }
    public Guid RatId { get; set; }
    public string RatCode { get; set; } = string.Empty;
    public string RatName { get; set; } = string.Empty;
    public string CageName { get; set; } = string.Empty;
    public DateTime DeathDate { get; set; }
    public DeathCause Cause { get; set; }
    public string? Note { get; set; }
}