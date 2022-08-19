using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TasksTracker.Common;
using static Microsoft.AspNetCore.Http.Results;

namespace TasksTracker.Tasks.GettingTask;

internal static class Route {
    internal static IEndpointRouteBuilder
    UseGetTaskEndpoint(this IEndpointRouteBuilder endpoints) {
        endpoints.MapGet("api/tasks/{id:guid}", async (HttpContext context, Guid id) => {
            var query = GetTaskQuery.Create(id);
            var result = await context.SendQuery<GetTaskQuery, TaskItem?>(query);
            return result != null ? Ok(result) : NotFound();
        });
        return endpoints;
    }
}