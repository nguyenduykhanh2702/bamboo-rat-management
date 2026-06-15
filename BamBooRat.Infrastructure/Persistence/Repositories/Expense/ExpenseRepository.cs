
using Microsoft.EntityFrameworkCore;

public class ExpenseRepository : IExpenseRepository
{
    private readonly AppDbContext _context;
    public ExpenseRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Expense expense)
    {
        await _context.AddAsync(expense);
    }

    public async Task<Expense?> GetByIdAsync(Guid id)
    {
        return await _context.Expenses.FirstOrDefaultAsync(x => x.Id == id);
    }

    public IQueryable<Expense> Query()
    {
        return _context.Expenses.AsQueryable();
    }

    public void Remove(Expense expense)
    {
        _context.Remove(expense);
    }

    public void Update(Expense expense)
    {
        _context.Update(expense);
    }
}