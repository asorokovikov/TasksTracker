using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace TasksTracker.Api.Middleware;

public sealed class ErrorHandlingMiddleware {
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next) => 
        _next = next;

    public async Task Invoke(HttpContext context) {
        try {
            await _next(context);
        }
        catch (Exception exception) {
            await HandleExceptionAsync(context, exception);
        }
    }

    public static Task HandleExceptionAsync(HttpContext context, Exception exception) {
        var result = JsonConvert.SerializeObject(new { error = exception.Message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)exception.ToHttpStatusCode();
        return context.Response.WriteAsync(result);
    }
}

internal static class ErrorHandlingHelper {
    internal static HttpStatusCode ToHttpStatusCode(this Exception exception) => exception switch {
        UnauthorizedAccessException _ => HttpStatusCode.Unauthorized,
        NotImplementedException _ => HttpStatusCode.NotImplemented,
        InvalidOperationException _ => HttpStatusCode.Conflict,
        InvalidDataException _ => HttpStatusCode.BadRequest,
        ArgumentException _ => HttpStatusCode.BadRequest,
        ValidationException _ => HttpStatusCode.BadRequest,
        KeyNotFoundException _ => HttpStatusCode.NotFound,
        _ => HttpStatusCode.InternalServerError
    };
}