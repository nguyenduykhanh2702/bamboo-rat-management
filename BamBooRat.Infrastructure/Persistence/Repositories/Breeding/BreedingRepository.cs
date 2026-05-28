
using Microsoft.EntityFrameworkCore;

public class BreedingRepository : IBreedingRepository
{
    private readonly AppDbContext _context;
    public BreedingRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Breeding breeding)
    {
        await _context.AddAsync(breeding);
    }

    public async Task<Breeding?> GetByIdAsync(Guid id)
    {
        return await _context.Breedings.FirstOrDefaultAsync(b => b.Id == id);
    }
    public IQueryable<Breeding> Query()
    {

        return _context.Breedings.AsQueryable();
    }

    public void Remove(Breeding breeding)
    {
        _context.Remove(breeding);
    }

    public void Update(Breeding breeding)
    {
        _context.Update(breeding);
    }
}