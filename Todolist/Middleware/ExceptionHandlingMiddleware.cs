using System.Text.Json;
using Todolist.Dto;
using Todolist.Exceptions;

namespace Todolist.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (HttpException ex) when (ex.StatusCode >= 500)
        {
            // 5xx → server-side problem, log as Error
            logger.LogError(ex, "Server error {StatusCode}: {Message}", ex.StatusCode, ex.Message);
            await WriteErrorResponse(context, ex.StatusCode, ex.Message, ex.Details);
        }
        catch (HttpException ex)
        {
            // 4xx → client-side problem, log as Warning
            logger.LogWarning(ex, "Client error {StatusCode}: {Message}", ex.StatusCode, ex.Message);
            await WriteErrorResponse(context, ex.StatusCode, ex.Message, ex.Details);
        }
        catch (ArgumentException ex)
        {
            // ArgumentException จาก Service/Helper → 400 Bad Request
            logger.LogWarning(ex, "Argument exception: {Message}", ex.Message);
            await WriteErrorResponse(context, 400, "Bad Request", ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await WriteErrorResponse(context, 500, "Internal Server Error", ex.Message);
        }
    }

    private static Task WriteErrorResponse(HttpContext context, int statusCode, string message, string? details)
    {
        var error = new Error
        {
            StatusCode = statusCode,
            Message = message,
            Details = details,
            Timestamp = DateTime.UtcNow
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var json = JsonSerializer.Serialize(error, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return context.Response.WriteAsync(json);
    }
}

// Extension method เพื่อ register ง่ายๆ ใน Program.cs
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionHandlingMiddleware>();
}

