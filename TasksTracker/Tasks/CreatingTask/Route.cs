using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TasksTracker.Common;
using static Microsoft.AspNetCore.Http.Results;

namespace TasksTracker.Tasks.CreatingTask;

internal record CreateTaskRequest(
    string? Name, 
    string? State
);

internal static class Route {
    // Please note that you cannot simultaneously create a new task and add attachments to it.
    // You can attach files only to an already existing task.
    internal static IEndpointRouteBuilder
    UseCreateTaskEndpoint(this IEndpointRouteBuilder endpoints) {
        // POST api/tasks
        endpoints.MapPost("api/tasks", async (HttpContext context, CreateTaskRequest request) => 
        {
            var taskId = Guid.NewGuid();
            var createTaskCommand = CreateTaskCommand.Create(
                id: taskId, 
                name: request.Name, 
                state: request.State,
                createdAt: DateTime.Now
            );

            await context.SendCommand(createTaskCommand);
            return Created($"/api/tasks/{taskId}", taskId);
        });

        return endpoints;
    }
}