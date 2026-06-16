public interface IDeathRecordService
{
    Task<DeathRecordDto> GetDeathRecordByIdAsync(Guid id);
    Task<PagedResult<DeathRecordDto>> GetAllDeathRecordsAsync(DeathRecordParams deathRecordParams);
    Task<DeathRecordDto> AddDeathRecordAsync(DeathRecordRequestDto dto);
}