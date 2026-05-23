public class RatDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public double Weight { get; set; }

    public RatType Type { get; set; }
    public RatStatus Status { get; set; }
    public Gender Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int? Age { get; set; }
    public string CageName { get; set; } = string.Empty;
    public Guid CageId { get; set; }
}