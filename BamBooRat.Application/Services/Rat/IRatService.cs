public interface IRatService
{
    Task<RatDetailDto> GetRatByIdAsync(Guid id);
    Task<RatDto> CreateAsync(CreateRatDto createRatDto);
    // Task UpdateAsync(Guid id, CreateCageDto cageDto);
    // Task DeleteAsync(Guid id);
    Task<PagedResult<RatDto>> GetPagedResultAsync(PaginationParams paginationParams);
}