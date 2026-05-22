public interface ICageService
{
    Task<CageDto> GetCageByIdAsync(Guid id);
    Task<CageDto> AddCageAsync(CreateCageDto cageDto);
    Task UpdateAsync(Guid id, CreateCageDto cageDto);
    Task DeleteAsync(Guid id);
    Task<PagedResult<CageDto>> GetPagedResultAsync(CageParams cageParams);
}