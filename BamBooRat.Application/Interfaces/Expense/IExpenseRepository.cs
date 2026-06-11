public interface IExpenseRepository
{
    IQueryable<Expense> Query();
    Task<Expense?> GetByIdAsync(Guid id);

    Task AddAsync(Expense expense);

    void Update(Expense expense);

    void Remove(Expense expense);
}