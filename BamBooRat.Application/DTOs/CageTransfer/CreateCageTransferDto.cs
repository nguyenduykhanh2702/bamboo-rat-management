public class CreateCageTransferDto
{
    public Guid RatId { get; set; }

    public Guid ToCageId { get; set; }

    public TransferReason Reason { get; set; }

    public string? Note { get; set; }
}