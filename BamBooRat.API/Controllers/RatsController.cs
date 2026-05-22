using Microsoft.AspNetCore.Mvc;

public class RatsController : BaseController
{
    private readonly IRatService _ratService;
    public RatsController(IRatService ratService)
    {
        _ratService = ratService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPagedResultAsync([FromQuery] RatParams ratParams)
    {
        var result = await _ratService.GetPagedResultAsync(ratParams);
        return Ok(new ApiResponse<PagedResult<RatDto>>(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRatByIdAsyncs(Guid id)
    {
        var rat = await _ratService.GetRatByIdAsync(id);
        return Ok(new ApiResponse<RatDetailDto>(rat));
    }
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateRatDto createRatDto)
    {
        var result = await _ratService.CreateAsync(createRatDto);
        return CreatedAtAction(nameof(GetRatByIdAsyncs),
        new { id = result.Id },
        new ApiResponse<RatDto>(result));
    }
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _ratService.DeleteAsync(id);
        return Ok(new ApiResponse<string>("Xóa dữ liệu thành công"));
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateRatDto ratDto)
    {
        await _ratService.UpdateAsync(id, ratDto);
        return Ok(new ApiResponse<string>("Cập nhập dữ liệu thành công"));
    }
}