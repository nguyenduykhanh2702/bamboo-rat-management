public interface ICageTransferService
{
    Task CreateAsync(Guid RatId, Guid sourceCageId, Guid destinationCageId, DateTime transferDate, TransferReason reason);
    Task<CageTransferDetailDto> GetByIdAsync(Guid id);
    Task<PagedResult<CageTransferDto>> GetAllCageTransferAsync(CageTransferParams cageTransferParams);
    Task DeleteAsync(Guid id);
}