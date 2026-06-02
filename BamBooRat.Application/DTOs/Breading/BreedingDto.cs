public class BreedingDto
{
    public Guid Id { get; set; }

    public Guid MaleId { get; set; }
    public string? MaleCode { get; set; }

    public Guid FemaleId { get; set; }
    public string? FemaleCode { get; set; }
    public Guid FemaleOldCageId { get; set; }
    public Guid CageId { get; set; }
    public string? CageName { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime ExpectedSeparationDate { get; set; }
    public DateTime? ActualSeparationDate { get; set; }

    public DateTime ExpectedBirthDate { get; set; }
    public DateTime? ActualBirthDate { get; set; }

    public bool? IsBirthSuccessful { get; set; }
    public int? OffspringCount { get; set; }
    public bool? IsOffspringSurvived { get; set; }
    public BreedingStatus BreedingStatus { get; set; }
}