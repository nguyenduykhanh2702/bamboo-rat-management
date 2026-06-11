public interface IExpenseService
{
    Task<ExpenseDetailDto> AddAsync(ExpenseRequestDto dto);
    Task<ExpenseDetailDto> GetExpenseByIdAsync(Guid id);
    Task<PagedResult<ExpenseDetailDto>> GetExpensePagedResultAsync(ExpenseParams expenseParams);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(Guid id, ExpenseRequestDto dto);
}