
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public ICageRepository CageRepository { get; }
    public UnitOfWork(AppDbContext context,
                    ICageRepository cageRepository)
    {
        _context = context;
        CageRepository = cageRepository;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}