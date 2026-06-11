public class ExpenseParams : PaginationParams
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}