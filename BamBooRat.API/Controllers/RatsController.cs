using Microsoft.AspNetCore.Mvc;

public class RatsController : BaseController
{
    private readonly IRatService _ratService;
    public RatsController(IRatService ratService)
    {
        _ratService = ratService;
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
}