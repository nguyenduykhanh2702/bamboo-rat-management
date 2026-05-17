public class Rat : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public RatType Type { get; set; }
    public RatStatus Status { get; set; }
    public Gender Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int? Age => DateOfBirth == null ? null : DateTime.UtcNow.Year - DateOfBirth.Value.Year;

    public Guid CageId { get; set; }
    public Cage Cage { get; set; } = null;
    
}