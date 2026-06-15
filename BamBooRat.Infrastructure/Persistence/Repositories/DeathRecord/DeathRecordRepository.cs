
using Microsoft.EntityFrameworkCore;

public class DeathRecordRepository : IDeathRecordRepository
{
    private readonly AppDbContext _context;
    public DeathRecordRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddDeathRecordAsync(DeathRecord deathRecord)
    {
        await _context.DeathRecords.AddAsync(deathRecord);
    }

    public void DeleteDeathRecord(DeathRecord deathRecord)
    {
        _context.DeathRecords.Remove(deathRecord);
    }

    public async Task<DeathRecord?> GetDeathRecordByIdAsync(Guid id)
    {
        return await _context.DeathRecords.FirstOrDefaultAsync(x => x.Id == id);
    }

    public IQueryable<DeathRecord> Query()
    {
        return _context.DeathRecords.AsQueryable();
    }

    public void UpdateDeathRecord(DeathRecord deathRecord)
    {
        _context.DeathRecords.Update(deathRecord);
    }
}