using System.Net;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TasksTracker.Api.Middleware;

public sealed class ErrorDetails {
    public HttpStatusCode StatusCode { get; }
    public string Message { get; }

    [JsonIgnore]
    public int StatusCodeNumber => (int)StatusCode;

    public ErrorDetails(HttpStatusCode statusCode, string message) {
        StatusCode = statusCode;
        Message = message;
    }

    public override string ToString() => JsonConvert.SerializeObject(this);
}

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
        var errorDetails = exception.ToErrorDetails();
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = errorDetails.StatusCodeNumber;
        return context.Response.WriteAsync(errorDetails.ToString());
    }
}

internal static class ErrorHandlingHelper {
    public static ErrorDetails ToErrorDetails(this Exception exception) => new (
        statusCode: exception.ToHttpStatusCode(),
        message: exception.Message
    );

    public static HttpStatusCode ToHttpStatusCode(this Exception exception) => exception switch {
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