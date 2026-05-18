using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CagesController : ControllerBase
{
    private readonly CageService _cageService;
    public CagesController(CageService cageService)
    {
        _cageService = cageService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllCages()
    {
        var cages = await _cageService.GetAllCagesAsync();
        return Ok(cages);
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCageById(Guid id)
    {
        var cage = await _cageService.GetCageByIdAsync(id);
        return Ok(cage);
    }
    [HttpPost]
    public async Task<IActionResult> CreateCageAsyn(CreateCageDto dto)
    {
        var result = await _cageService.AddCageAsync(dto);
        return CreatedAtAction(nameof(GetCageById), new { id = result.Id }, result);
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCageAsync(Guid id, CreateCageDto dto)
    {
        await _cageService.UpdateAsync(id, dto);
        return NoContent(); // 204
    }
    [HttpPost("{id:guid}")]
    public async Task<IActionResult> DeleteCageAsync(Guid id)
    {
        await _cageService.DeleteAsync(id);
        return NoContent(); // 204
    }
}