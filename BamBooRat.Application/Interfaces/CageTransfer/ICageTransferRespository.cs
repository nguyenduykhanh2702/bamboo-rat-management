public interface ICageTransferRespository
{
    IQueryable<CageTransfer> Query();
    Task<CageTransfer?> GetByIdAsync(Guid id);

    Task AddAsync(CageTransfer cageTransfer);

    void Update(CageTransfer cageTransfer);

    void Remove(CageTransfer cageTransfer);
}