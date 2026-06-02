public class CageTransferDto
{
    public Guid Id { get; set; }

    public Guid RatId { get; set; }

    public string RatCode { get; set; } = string.Empty;
    public string RatName { get; set; } = string.Empty;

    public Guid? FromCageId { get; set; }

    public string? FromCageName { get; set; }

    public Guid ToCageId { get; set; }

    public string ToCageName { get; set; } = string.Empty;

    public TransferReason Reason { get; set; }

    public DateTime TransferDate { get; set; }

    public string? Note { get; set; }
}