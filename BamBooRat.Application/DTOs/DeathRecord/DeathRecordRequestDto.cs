public class DeathRecordRequestDto
{
    public Guid RatId { get; set; }
    public DateTime DeathDate { get; set; }
    public DeathCause Cause { get; set; }
    public string? Note { get; set; }
}