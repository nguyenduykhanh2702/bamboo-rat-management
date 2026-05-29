public class ConFirmBirthDto
{
    public Guid BreedingId { get; set; }
    public bool IsBirthSuccessful { get; set; }
    public int? OffspringCount { get; set; }
    public bool? IsOffspringSurvived { get; set; }
    public DateTime? ActualBirthDate { get; set; }
    public string? Notes { get; set; }

}