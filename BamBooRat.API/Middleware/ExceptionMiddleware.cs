using System.Net;
using System.Text.Json;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status404NotFound
                && !context.Response.HasStarted)
            {
                await HandleNotFoundRouteAsync(context);
            }
        }

        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var traceId = context.TraceIdentifier;

        var response = ex switch
        {
            ValidationExceptionCustom valEx => new ApiErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Validation failed",
                Errors = valEx.Errors,
                TraceId = traceId
            },

            NotFoundException => new ApiErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = ex.Message,
                TraceId = traceId
            },

            UnauthorizedAccessException => new ApiErrorResponse
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Message = "Unauthorized",
                TraceId = traceId
            },

            ArgumentException => new ApiErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = ex.Message,
                TraceId = traceId
            },

            _ => new ApiErrorResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Internal server error",
                TraceId = traceId
            }
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = response.StatusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }

    private static Task HandleNotFoundRouteAsync(HttpContext context)
    {
        var response = new
        {
            statusCode = 404,
            Message = "Route not found",
            TraceId = context.TraceIdentifier
        };

        context.Response.ContentType = "application/json";

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}