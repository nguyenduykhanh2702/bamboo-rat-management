public interface IDeathRecordRepository
{
    Task<DeathRecord?> GetDeathRecordByIdAsync(Guid id);
    IQueryable<DeathRecord> Query();
    Task AddDeathRecordAsync(DeathRecord deathRecord);
    void UpdateDeathRecord(DeathRecord deathRecord);
    void DeleteDeathRecord(DeathRecord deathRecord);
}