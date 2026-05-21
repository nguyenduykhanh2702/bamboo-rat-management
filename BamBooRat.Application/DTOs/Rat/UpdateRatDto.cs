public class UpdateRatDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public RatType Type { get; set; }
    public RatStatus Status { get; set; }
    public Gender Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public Guid CageId { get; set; }
}