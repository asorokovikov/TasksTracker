using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TasksTracker.Common;
using static Microsoft.AspNetCore.Http.Results;

namespace TasksTracker.Tasks.RemovingTask;

internal static class Route {
    internal static IEndpointRouteBuilder
    UseRemoveTaskEndpoint(this IEndpointRouteBuilder endpoints) {
        // DELETE api/tasks
        endpoints.MapDelete("api/tasks/{id:guid}", async (HttpContext context, Guid id) => {
            var command = RemoveTaskCommand.Create(id);
            await context.SendCommand(command);
            return Ok();
        });

        return endpoints;
    }
}