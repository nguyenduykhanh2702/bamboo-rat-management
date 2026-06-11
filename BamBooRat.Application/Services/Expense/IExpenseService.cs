public interface IExpenseService
{
    Task<ExpenseDetailDto> AddAsync(CreateExpenseDto dto);
    Task<ExpenseDetailDto> GetExpenseByIdAsync(Guid id);
}