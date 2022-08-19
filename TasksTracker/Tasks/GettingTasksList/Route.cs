using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TasksTracker.Common;
using static Microsoft.AspNetCore.Http.Results;

namespace TasksTracker.Tasks.GettingTasksList;

internal static class Route {
    internal static IEndpointRouteBuilder
    UseGetTasksListEndpoint(this IEndpointRouteBuilder endpoints) {
        endpoints.MapGet(
            pattern: "api/tasks",
            handler: async (HttpContext context, [FromQuery] int? page, [FromQuery] int? pageSize, [FromQuery] string? filter) => {
                var query = GetTasksListQuery.Create(page: page, pageSize: pageSize, filter: filter);
                var result = await context.SendQuery<GetTasksListQuery, IReadOnlyList<TaskItem>>(query);
                return Ok(result);
            }
        );
        return endpoints;
    }
}