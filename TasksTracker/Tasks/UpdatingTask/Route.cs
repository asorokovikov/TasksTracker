using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TasksTracker.Common;
using static Microsoft.AspNetCore.Http.Results;

namespace TasksTracker.Tasks.UpdatingTask;

internal record UpdateTaskRequest(
    Guid? TaskId,
    string? Name,
    string? State,
    DateTime? CreatedAt
);

internal static class Route {
    internal static IEndpointRouteBuilder
    UseUpdateTaskEndpoint(this IEndpointRouteBuilder endpoints) {
        // PUT api/tasks
        endpoints.MapPut(
            pattern: "api/tasks/{id:guid}",
            handler: async (HttpContext context, Guid id, UpdateTaskRequest request) => {
                if (id != request.TaskId)
                    return BadRequest();
                var command = UpdateTaskCommand.Create(
                    id: request.TaskId,
                    name: request.Name,
                    state: request.State,
                    createdAt: request.CreatedAt
                );
                await context.SendCommand(command);
                return Ok();
            });
        return endpoints;
    }
}