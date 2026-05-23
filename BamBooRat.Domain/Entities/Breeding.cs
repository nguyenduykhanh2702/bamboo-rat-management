public class Breeding : BaseEntity
{
    public Guid MaleId { get; set; }
    public Rat Male { get; set; } = null!;

    public Guid FemaleId { get; set; }
    public Rat Female { get; set; } = null!;

    public Guid CageId { get; set; }
    public Cage Cage { get; set; } = null!;

    public Guid? OriginalFemaleCageId { get; set; }

    public DateTime BreedingDate { get; set; }
    public DateTime? ExpectedBirthDate { get; set; }
    public DateTime? ActualBirthDate { get; set; }

    public int? OffspringCount { get; set; }
    public string? Notes { get; set; }
}