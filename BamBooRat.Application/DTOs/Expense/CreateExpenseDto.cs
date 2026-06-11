public class CreateExpenseDto
{
    public DateTime ExpenseDate { get; set; }
    public ExpenseType Type { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Note { get; set; }
}