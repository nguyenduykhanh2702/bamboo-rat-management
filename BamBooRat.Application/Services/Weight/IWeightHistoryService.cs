public interface IWeightHistoryService
{
    Task<PagedResult<WeightHistoryDto>> GetWeightHistoryPageResult(WeightHistoryParams weightHistoryParams);
}