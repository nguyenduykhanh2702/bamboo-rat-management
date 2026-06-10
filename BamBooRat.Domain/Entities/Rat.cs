public class Rat : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public double Weight { get; set; }
    public string? Description { get; set; }
    public RatType Type { get; set; }
    public RatStatus Status { get; set; }
    public Gender Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? LastSeparationDate { get; set; }
    public Guid CageId { get; set; }
    public Cage Cage { get; set; } = null!;
    public ICollection<Breeding> MaleBreedings { get; set; } = new List<Breeding>();
    public ICollection<Breeding> FemaleBreedings { get; set; } = new List<Breeding>();
    public ICollection<CageTransfer> CageTransfers { get; set; } = new List<CageTransfer>();
    public ICollection<WeightHistory> WeightHistories { get; set; } = new List<WeightHistory>();
    public ICollection<HealthRecord> HealthRecords { get; set; } = new List<HealthRecord>();

}