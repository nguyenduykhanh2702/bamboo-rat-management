public class CageTransfer : BaseEntity
{
    public Guid RatId { get; set; }

    public Rat Rat { get; set; } = null!;

    public Guid? FromCageId { get; set; }

    public Cage FromCage { get; set; } = null!;

    public Guid ToCageId { get; set; }

    public Cage ToCage { get; set; } = null!;

    public TransferReason Reason { get; set; }

    public DateTime TransferDate { get; set; }

    public string? Note { get; set; }
}