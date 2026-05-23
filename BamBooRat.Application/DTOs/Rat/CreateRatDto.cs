public class CreateRatDto
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string? Description { get; set; }
    public double Weight { get; set; }

    public RatType Type { get; set; }
    public Gender Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public Guid CageId { get; set; }
}