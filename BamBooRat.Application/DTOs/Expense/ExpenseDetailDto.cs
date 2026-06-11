public class ExpenseDetailDto
{
    public Guid Id { get; set; }
    public DateTime ExpenseDate { get; set; }
    public ExpenseType Type { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Note { get; set; }
    public decimal Amount => Quantity * UnitPrice;
}