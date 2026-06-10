public interface IHealthRecordRepository
{
    IQueryable<HealthRecord> Query();
    Task<HealthRecord?> GetByIdAsync(Guid id);

    Task AddAsync(HealthRecord healthRecord);

    void Update(HealthRecord healthRecord);

    void Remove(HealthRecord healthRecord);
}