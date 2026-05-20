
using Microsoft.EntityFrameworkCore;

public class CageRepository : ICageRepository
{
    private readonly AppDbContext _context;
    public CageRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Cage cage)
    {
        await _context.AddAsync(cage);
    }

    public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null)
    {
        var normalized = name.Trim().ToLower();

        return await _context.Cages.AnyAsync(c =>
            c.Name.ToLower() == normalized &&
            (!excludeId.HasValue || c.Id != excludeId.Value));
    }

    public async Task<List<Cage>> GetAllAsync()
    {
        return await _context.Cages.ToListAsync();
    }

    public async Task<Cage?> GetByIdAsync(Guid id)
    {
        return await _context.Cages.FindAsync(id);
    }

    public void Remove(Cage cage)
    {
        _context.Remove(cage);
    }

    public void Update(Cage cage)
    {
        _context.Update(cage);
    }
}