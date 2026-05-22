public interface IRatService
{
    Task<RatDetailDto> GetRatByIdAsync(Guid id);
    Task<RatDto> CreateAsync(CreateRatDto createRatDto);
    Task UpdateAsync(Guid id, UpdateRatDto ratDto);
    Task DeleteAsync(Guid id);
    Task<PagedResult<RatDto>> GetPagedResultAsync(RatParams ratParams);
}