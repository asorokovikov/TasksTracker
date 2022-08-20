using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TasksTracker.Common;
using static Microsoft.AspNetCore.Http.Results;

namespace TasksTracker.Tasks.Attachments.GettingAttachmentsList;

internal static class Route {
    internal static IEndpointRouteBuilder
    UseGetAttachmentsListEndpoint(this IEndpointRouteBuilder endpoints) {
        // GET api/tasks/{taskId}/attachments
        var route = endpoints.MapGet(
            pattern: "api/tasks/{taskId:guid}/attachments",
            handler: async (HttpContext context, Guid taskId) =>
            {
                var query = GetAttachmentsListQuery.Create(taskId: taskId);
                var result = await context.SendQuery<GetAttachmentsListQuery, IReadOnlyList<AttachmentItem>>(query);
                return Ok(result);
            });

        return endpoints;
    }
}
