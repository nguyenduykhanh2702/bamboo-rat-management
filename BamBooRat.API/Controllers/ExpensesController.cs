using Microsoft.AspNetCore.Mvc;

public class ExpensesController : BaseController
{
    private readonly IExpenseService _expenseService;
    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _expenseService.GetExpenseByIdAsync(id);
        return Ok(new ApiResponse<ExpenseDetailDto>(result));
    }
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateExpenseDto dto)
    {
        var result = await _expenseService.AddAsync(dto);
        return CreatedAtAction(nameof(GetById),
                new { id = result.Id },
                new ApiResponse<ExpenseDetailDto>(result));
    }
    [HttpGet]
    public async Task<IActionResult> GetPageAsync([FromQuery] ExpenseParams expenseParams)
    {
        var result = await _expenseService.GetExpensePagedResultAsync(expenseParams);
        return Ok(new ApiResponse<PagedResult<ExpenseDetailDto>>(result));
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateExpeseDto dto)
    {
        await _expenseService.UpdateAsync(id, dto);
        return Ok(new ApiResponse<string>("Cập nhật dữ liệu thành công"));
    }
    [HttpDelete]
    public async Task<IActionResult> DeleteAysnc(Guid id)
    {
        await _expenseService.DeleteAsync(id);
        return Ok(new ApiResponse<string>("Xoá dữ liệu thành công"));
    }
}