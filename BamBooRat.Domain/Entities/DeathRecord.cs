public class DeathRecord : AuditableEntity
{
    public Guid RatId { get; set; }
    public Rat Rat { get; set; } = null!;
    public Guid? CageId { get; set; }
    public Cage? Cage { get; set; }
    public DateTime DeathDate { get; set; }
    public DeathCause Cause { get; set; }
    public string? Note { get; set; }
}