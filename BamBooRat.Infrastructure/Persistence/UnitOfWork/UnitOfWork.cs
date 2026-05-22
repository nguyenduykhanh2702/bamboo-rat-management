
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public ICageRepository CageRepository { get; }
    public IRatRespository RatRespository { get; }

    public UnitOfWork(AppDbContext context,
                    ICageRepository cageRepository,
                    IRatRespository ratRepository)
    {
        _context = context;
        CageRepository = cageRepository;
        RatRespository = ratRepository;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}