using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TasksTracker.Common;
using static Microsoft.AspNetCore.Http.Results;

namespace TasksTracker.Tasks.Attachments.RemovingAttachment;

internal static class Route {
    internal static IEndpointRouteBuilder
    UseRemoveAttachmentEndpoint(this IEndpointRouteBuilder endpoints) {
        // DELETE api/tasks/{taskId}/attachments/{fileId}
        var route = endpoints.MapDelete(
            pattern: "api/tasks/{taskId:guid}/attachments/{fileId:guid}",
            handler: async (HttpContext context, Guid taskId, Guid fileId) => {
                await context.SendCommand(RemoveAttachmentCommand.Create(fileId: fileId, taskId: taskId));
                return Ok();
        });

        return endpoints;
    }
}
