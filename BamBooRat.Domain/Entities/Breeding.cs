public class Breeding : AuditableEntity, ISoftDeletable
{
    public Guid MaleId { get; set; }
    public Rat Male { get; set; } = null!;

    public Guid FemaleId { get; set; }
    public Rat Female { get; set; } = null!;

    public Guid CageId { get; set; }
    public Cage Cage { get; set; } = null!;

    public Guid FemaleOldCageId { get; set; }
    public Cage FemaleOldCage { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime ExpectedSeparationDate { get; set; }
    public DateTime ExpectedBirthDate { get; set; }

    public DateTime? ActualSeparationDate { get; set; }

    public bool? IsBirthSuccessful { get; set; }
    public DateTime? ActualBirthDate { get; set; }

    public int? OffspringCount { get; set; }

    public bool? IsOffspringSurvived { get; set; }   // con có sống không

    public string? Notes { get; set; }

    public BreedingStatus BreedingStatus { get; set; } = BreedingStatus.Breeding;
    public bool IsDeleted { get; set; }
}