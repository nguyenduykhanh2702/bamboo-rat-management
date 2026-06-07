using Microsoft.AspNetCore.Mvc;

public class WeightHistoriesController : BaseController
{
    private readonly IWeightHistoryService _weightHistoryService;

    public WeightHistoriesController(IWeightHistoryService weightHistoryService)
    {
        _weightHistoryService = weightHistoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetWeightHistoryPageResult([FromQuery] WeightHistoryParams weightHistoryParams)
    {
        var result = await _weightHistoryService.GetWeightHistoryPageResult(weightHistoryParams);
        return Ok(new ApiResponse<PagedResult<WeightHistoryDto>>(result));
    }
    [HttpPost]
    public async Task<IActionResult> CraeteWeightHistoryAsync(CreateWeightHistoryDto weightHistoryDto)
    {
        var result = await _weightHistoryService.CreateWeightHistoryParams(weightHistoryDto);
        return Ok(new ApiResponse<WeightHistoryDto>(result));
    }
}