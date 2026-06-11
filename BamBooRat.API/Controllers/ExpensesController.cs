using Microsoft.AspNetCore.Mvc;

public class ExpensesController : BaseController
{
    private readonly IExpenseService _expenseService;
    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetExpenseById(Guid id)
    {
        var result = await _expenseService.GetExpenseByIdAsync(id);
        return Ok(new ApiResponse<ExpenseDetailDto>(result));
    }
    [HttpPost]
    public async Task<IActionResult> CreateEpenseAsync([FromBody] CreateExpenseDto dto)
    {
        var result = await _expenseService.AddAsync(dto);
        return CreatedAtAction(nameof(GetExpenseById),
                new { id = result.Id },
                new ApiResponse<ExpenseDetailDto>(result));
    }
}