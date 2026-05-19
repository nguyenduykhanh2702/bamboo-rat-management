public class ApiErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public object Errors { get; set; }
    public string TraceId { get; set; }
}