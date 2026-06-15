public interface IDeathRecordService
{
    Task<DeathRecordDto> GetDeathRecordByIdAsync(Guid id);
    // Task<IEnumerable<DeathRecord>> GetAllDeathRecordsAsync();
    Task<DeathRecordDto> AddDeathRecordAsync(DeathRecordRequestDto dto);
    // Task UpdateDeathRecordAsync(Guid Id, DeathRecord deathRecord);
    // Task DeleteDeathRecordAsync(Guid id);
}