
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public ICageRepository CageRepository { get; }
    public IRatRespository RatRespository { get; }
    public IBreedingRepository BreedingRepository { get; }

    public UnitOfWork(AppDbContext context,
                    ICageRepository cageRepository,
                    IRatRespository ratRepository,
                    IBreedingRepository breedingRepository)
    {
        _context = context;
        CageRepository = cageRepository;
        RatRespository = ratRepository;
        BreedingRepository = breedingRepository;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}