using Microsoft.AspNetCore.Mvc;

public class HealthRecordsController : BaseController
{
    private readonly IHealthRecordService _healthRecordService;

    public HealthRecordsController(IHealthRecordService healthRecordService)
    {
        _healthRecordService = healthRecordService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateHealthRecordAsync([FromBody] CreateHealthRecordDto dto)
    {
        var result = await _healthRecordService.CreateAsync(dto);
        Console.WriteLine($"Result Id: {result.Id}");
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
        new ApiResponse<HealthRecordDetailDto>(result));
    }
    public async Task<IActionResult> GetAll([FromQuery] HealthRecordParams healthParams)
    {
        var result = await _healthRecordService.GetAllAsync(healthParams);
        return Ok(new ApiResponse<PagedResult<HealthRecordDto>>(result));
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _healthRecordService.GetByIdAsync(id);
        return Ok(new ApiResponse<HealthRecordDetailDto>(result));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHealthRecordDto dto)
    {
        await _healthRecordService.UpdateAsync(id, dto);
        return Ok(new ApiResponse<string>("Cập nhật bản ghi sức khỏe thành công"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _healthRecordService.DeleteAsync(id);
        return NoContent();
    }
}