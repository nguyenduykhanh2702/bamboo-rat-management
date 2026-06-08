public interface IWeightHistoryRepository
{
    IQueryable<WeightHistory> Query();
    Task<WeightHistory?> GetByIdAsync(Guid id);

    Task AddAsync(WeightHistory weightHistory);

    void Update(WeightHistory weightHistory);

    void Remove(WeightHistory weightHistory);
}