public interface IUnitOfWork
{
    ICageRepository CageRepository { get; }
    IRatRespository RatRespository { get; }
    IBreedingRepository BreedingRepository { get; }
    ICageTransferRespository CageTransferRespository { get; }
    IWeightHistoryRepository WeightHistoryRepository { get; }
    Task<int> SaveChangesAsync();
}