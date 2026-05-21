
using Microsoft.EntityFrameworkCore;

public class RatRespository : IRatRespository
{
    private readonly AppDbContext _context;
    public RatRespository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Rat rat)
    {
        await _context.AddAsync(rat);
    }

    public async Task<Rat?> GetByIdAsync(Guid id)
    {
        return await _context.Rats.FindAsync(id);
    }

    public async Task<List<string>> GetDuplicateFieldsAsync(string name, string code, Guid? excludeId = null)
    {
        var query = _context.Rats.AsQueryable();

        if (excludeId.HasValue)
            query = query.Where(x => x.Id != excludeId.Value);

        var result = new List<string>();

        if (await query.AnyAsync(x => x.Name == name))
            result.Add("name");

        if (await query.AnyAsync(x => x.Code == code))
            result.Add("code");

        return result;
    }

    public IQueryable<Rat> Query()
    {
        return _context.Rats.AsNoTracking();
    }

    public void Remove(Rat rat)
    {
        _context.Rats.Remove(rat);
    }

    public void Update(Rat rat)
    {
        _context.Update(rat);
    }
}