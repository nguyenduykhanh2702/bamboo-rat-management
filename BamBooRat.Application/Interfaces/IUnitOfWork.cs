public interface IUnitOfWork
{
    ICageRepository CageRepository { get; }
    IRatRespository RatRespository { get; }
    IBreedingRepository BreedingRepository { get; }
    Task<int> SaveChangesAsync();
}