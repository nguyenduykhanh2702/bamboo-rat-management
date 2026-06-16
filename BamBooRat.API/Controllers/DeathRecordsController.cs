using Microsoft.AspNetCore.Mvc;

public class DeathRecordsController : BaseController
{
    private readonly IDeathRecordService _deathRecordService;

    public DeathRecordsController(IDeathRecordService deathRecordService)
    {
        _deathRecordService = deathRecordService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetDeathRecordById(Guid id)
    {
        var deathRecords = await _deathRecordService.GetDeathRecordByIdAsync(id);
        return Ok(new ApiResponse<DeathRecordDto>(deathRecords));
    }
    [HttpGet]
    public async Task<IActionResult> GetDeathRecordPageAsync([FromQuery] DeathRecordParams deathRecordParams)
    {
        var result = await _deathRecordService.GetAllDeathRecordsAsync(deathRecordParams);
        return Ok(new ApiResponse<PagedResult<DeathRecordDto>>(result));
    }
    [HttpPost]
    public async Task<IActionResult> CreateDeathRecordAsync([FromBody] DeathRecordRequestDto dto)
    {
        var result = await _deathRecordService.AddDeathRecordAsync(dto);
        return CreatedAtAction(nameof(GetDeathRecordById),
                             new { id = result.Id },
                              new ApiResponse<DeathRecordDto>(result));
    }
}