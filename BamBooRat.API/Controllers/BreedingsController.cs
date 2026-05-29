using Microsoft.AspNetCore.Mvc;

public class BreedingsController : BaseController
{
    private readonly IBreedingService _breedingService;
    public BreedingsController(IBreedingService breedingService)
    {
        _breedingService = breedingService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBreedingAsync([FromBody] CreateBreedingDto dto)
    {
        var result = await _breedingService.CreateBreedingAsync(dto);
        return CreatedAtAction(nameof(GetBreedingById),
        new { id = result.Id },
        new ApiResponse<BreedingDto>(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBreedingById(Guid id)
    {
        var breeding = await _breedingService.GetBreedingByIdAsync(id);
        return Ok(new ApiResponse<BreedingDto>(breeding));
    }

    [HttpPut("{breedingId:guid}/confirm-birth")]
    public async Task<IActionResult> ConFirmBirthAsync([FromBody] ConFirmBirthDto dto, Guid breedingId)
    {
        await _breedingService.ConFirmBirthAsync(breedingId, dto);
        return Ok(new ApiResponse<string>("Xác nhận sinh sản thành công"));
    }
    [HttpPut("{breedingId:guid}/separate")]
    public async Task<IActionResult> SpreatBreedingAsync(Guid breedingId)
    {
        await _breedingService.SpreatBreedingAsync(breedingId);
        return Ok(new ApiResponse<string>("Tách phối thành công"));
    }
}