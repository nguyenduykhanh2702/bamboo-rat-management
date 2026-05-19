public interface IUnitOfWork
{
    ICageRepository CageRepository { get; }

    Task<int> SaveChangesAsync();
}