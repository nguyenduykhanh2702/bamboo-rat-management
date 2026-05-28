public interface IBreedingService
{
    Task<BreedingDto> CreateBreedingAsync(CreateBreedingDto dto);
    Task<BreedingDto> GetBreedingByIdAsync(Guid id);
    Task UpdateAsync(Guid id, CreateBreedingDto dto);
    Task DeleteAsync(Guid id);

}