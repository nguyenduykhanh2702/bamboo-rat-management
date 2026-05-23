public class BreedingDto
{
    public Guid Id { get; set; }
    public string MaleCode { get; set; } = string.Empty;
    public string FemaleCode { get; set; } = string.Empty;
    public string CageName { get; set; } = string.Empty;

    public DateTime BreedingDate { get; set; }
    public DateTime? ExpectedBirthDate { get; set; }
    public DateTime? ActualBirthDate { get; set; }
}