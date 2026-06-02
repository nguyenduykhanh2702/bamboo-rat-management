public interface IBreedingService
{
    Task<BreedingDto> CreateBreedingAsync(CreateBreedingDto dto);
    Task<BreedingDto> GetBreedingByIdAsync(Guid id);
    Task<PagedResult<BreedingDto>> GetBreedingsAsync(BreedingParams breedingParams);
    Task SpreatBreedingAsync(Guid breedingId);
    Task ConfirmPregnancyAsync(Guid breedingId, ConfirmPregnancyDto dto);
    Task ConFirmBirthAsync(Guid breedingId, ConFirmBirthDto dto);
    Task UpdateOffSpringStatusAsync(Guid breedingId, UpdateOffSpringStatusDto dto);
    Task CancelBreedingAsync(Guid breedingId);
}