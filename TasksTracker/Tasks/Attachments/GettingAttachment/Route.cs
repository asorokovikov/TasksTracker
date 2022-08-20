using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TasksTracker.Common;
using static Microsoft.AspNetCore.Http.Results;

namespace TasksTracker.Tasks.Attachments.GettingAttachment;

internal static class Route {
    internal static IEndpointRouteBuilder
    UseGetAttachmentEndpoint(this IEndpointRouteBuilder endpoints) {
        // GET api/tasks/{taskId}/attachments/{fileId}
        var route = endpoints.MapGet(
            pattern: "api/tasks/{taskId:guid}/attachments/{fileId:guid}",
            handler: async (HttpContext context, Guid taskId, Guid fileId) => {
                var query = GetAttachmentQuery.Create(taskId: taskId, fileId: fileId);
                var result = await context.SendQuery<GetAttachmentQuery, FileItem>(query);
                return File(result.Stream, "application/octet-stream", result.Filename);
            });

        return endpoints;
    }
}
