public interface IWeightHistoryService
{
    Task<PagedResult<WeightHistoryDto>> GetWeightHistoryPageResult(WeightHistoryParams weightHistoryParams);
    Task<WeightHistoryDto> CreateWeightHistoryParams(CreateWeightHistoryDto weightHistoryDto);
}