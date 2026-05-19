public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }

    public ApiResponse(T data)
    {
        Success = true;
        Data = data;
    }
}