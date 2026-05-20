using Microsoft.AspNetCore.Mvc;
public class CagesController : BaseController
{
    private readonly CageService _cageService;
    public CagesController(CageService cageService)
    {
        _cageService = cageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCagesPaged([FromQuery] PaginationParams paginationParams)
    {
        var pagedResult = await _cageService.GetPagedResultAsync(paginationParams);
        return Ok(new ApiResponse<PagedResult<CageDto>>(pagedResult));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCageById(Guid id)
    {
        var cage = await _cageService.GetCageByIdAsync(id);
        return Ok(new ApiResponse<CageDto>(cage));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCageAsync([FromBody] CreateCageDto dto)
    {
        var result = await _cageService.AddCageAsync(dto);
        return CreatedAtAction(nameof(GetCageById),
        new { id = result.Id },
        new ApiResponse<CageDto>(result));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCageAsync(Guid id, [FromBody] CreateCageDto dto)
    {
        await _cageService.UpdateAsync(id, dto);
        return Ok(new ApiResponse<string>("Cập nhập dữ liệu thành công"));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCageAsync(Guid id)
    {
        await _cageService.DeleteAsync(id);
        return Ok(new ApiResponse<string>("Thêm mới dữ liệu thành công"));
    }
}