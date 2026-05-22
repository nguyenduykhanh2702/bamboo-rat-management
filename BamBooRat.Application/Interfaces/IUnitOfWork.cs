public interface IUnitOfWork
{
    ICageRepository CageRepository { get; }
    IRatRespository RatRespository { get; }
    Task<int> SaveChangesAsync();
}