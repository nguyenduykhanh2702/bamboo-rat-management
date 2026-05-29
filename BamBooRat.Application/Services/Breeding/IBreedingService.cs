public interface IBreedingService
{
    Task<BreedingDto> CreateBreedingAsync(CreateBreedingDto dto);
    Task<BreedingDto> GetBreedingByIdAsync(Guid id);
    Task SpreatBreedingAsync(Guid breedingId);
    Task ConfirmPregnancyAsync(Guid breedingId, ConfirmPregnancyDto dto);
    Task ConFirmBirthAsync(Guid breedingId, ConFirmBirthDto dto);

    Task CancelBreedingAsync(Guid breedingId);
}