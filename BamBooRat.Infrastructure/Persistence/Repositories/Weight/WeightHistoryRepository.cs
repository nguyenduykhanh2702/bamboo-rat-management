

using Microsoft.EntityFrameworkCore;

public class WeightHistoryRepository : IWeightHistoryRepository
{
    private readonly AppDbContext _dbContext;
    public WeightHistoryRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(WeightHistory weightHistory)
    {
        await _dbContext.weightHistories.AddAsync(weightHistory);
    }

    public async Task<WeightHistory?> GetByIdAsync(Guid id)
    {
        return await _dbContext.weightHistories.FirstOrDefaultAsync(x => x.Id == id);
    }

    public IQueryable<WeightHistory> Query()
    {
        return _dbContext.weightHistories.AsQueryable();
    }

    public void Remove(WeightHistory weightHistory)
    {
        _dbContext.weightHistories.Remove(weightHistory);
    }

    public void Update(WeightHistory weightHistory)
    {
        _dbContext.Update(weightHistory);
    }
}