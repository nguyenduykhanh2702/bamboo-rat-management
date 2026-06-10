public interface IHealthRecordService
{
    Task<HealthRecordDetailDto> GetByIdAsync(Guid id);
    Task<PagedResult<HealthRecordDto>> GetAllAsync(HealthRecordParams healthParams);
    Task<HealthRecordDetailDto> CreateAsync(CreateHealthRecordDto dto);
    Task UpdateAsync(Guid id, UpdateHealthRecordDto dto);
    Task DeleteAsync(Guid id);
}