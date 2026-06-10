
public class HealthRecordRepository : IHealthRecordRepository
{
    private readonly AppDbContext _context;
    public HealthRecordRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(HealthRecord healthRecord)
    {
        await _context.AddAsync(healthRecord);
    }

    public async Task<HealthRecord?> GetByIdAsync(Guid id)
    {
        return await _context.HealthRecords.FindAsync(id);
    }

    public IQueryable<HealthRecord> Query()
    {
        return _context.HealthRecords.AsQueryable();
    }

    public void Remove(HealthRecord healthRecord)
    {
        _context.Remove(healthRecord);
    }

    public void Update(HealthRecord healthRecord)
    {
        _context.Update(healthRecord);
    }
}