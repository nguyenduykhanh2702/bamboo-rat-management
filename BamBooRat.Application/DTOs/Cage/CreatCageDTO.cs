public class CreateCageDto
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }
    public CageType Type { get; set; }

}