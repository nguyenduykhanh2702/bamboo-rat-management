using Microsoft.AspNetCore.Mvc;

public class CageTransfersController : BaseController
{
    private readonly ICageTransferService _cageTransferService;
    public CageTransfersController(ICageTransferService cageTransferService)
    {
        _cageTransferService = cageTransferService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCageTransferById(Guid id)
    {
        var transfer = await _cageTransferService.GetByIdAsync(id);
        return Ok(new ApiResponse<CageTransferDetailDto>(transfer));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCageTransferAsync([FromQuery] CageTransferParams cageTransferParams)
    {
        var transfers = await _cageTransferService.GetAllCageTransferAsync(cageTransferParams);
        return Ok(new ApiResponse<PagedResult<CageTransferDto>>(transfers));
    }
}