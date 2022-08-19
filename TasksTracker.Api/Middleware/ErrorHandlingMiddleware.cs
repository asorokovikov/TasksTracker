using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace TasksTracker.Api.Middleware;

public sealed class ErrorHandlingMiddleware {
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory) {
        _next = next;
        _logger = loggerFactory.CreateLogger<ExceptionHandlerMiddleware>();
    }

    public async Task Invoke(HttpContext context) {
        try {
            await _next(context);
        }
        catch (Exception exception) {
            await HandleExceptionAsync(context, exception);
        }
    }

    public Task HandleExceptionAsync(HttpContext context, Exception exception) {
        // _logger.LogError(exception, "{Message}", exception.Message);
        // Console.WriteLine("ERROR:" + exception.Message + exception.StackTrace);
        // if(exception.InnerException != null)
        //     Console.WriteLine("INNER DETAILS:" + exception.InnerException.Message + exception.InnerException.StackTrace);
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