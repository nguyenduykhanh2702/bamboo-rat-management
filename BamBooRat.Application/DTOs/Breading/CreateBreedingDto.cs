public class CreateBreedingDto
{
    public Guid MaleId { get; set; }
    public Guid FemaleId { get; set; }
    //public Guid CageId { get; set; }
    public DateTime StartDate { get; set; }
    public string? Notes { get; set; }
}