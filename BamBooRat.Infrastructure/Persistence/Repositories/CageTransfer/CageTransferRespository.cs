
using System.Threading.Tasks;

public class CageTransferRespository : ICageTransferRespository
{
    private readonly AppDbContext _context;
    public CageTransferRespository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(CageTransfer cageTransfer)
    {
        await _context.AddAsync(cageTransfer);
    }

    public async Task<CageTransfer?> GetByIdAsync(Guid id)
    {
        return await _context.CageTransfers.FindAsync(id);
    }

    public IQueryable<CageTransfer> Query()
    {
        return _context.CageTransfers.AsQueryable();
    }

    public void Remove(CageTransfer cageTransfer)
    {
        _context.Remove(cageTransfer);
    }

    public void Update(CageTransfer cageTransfer)
    {
        _context.Update(cageTransfer);
    }
}