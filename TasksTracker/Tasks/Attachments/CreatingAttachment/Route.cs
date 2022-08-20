using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TasksTracker.Common;
using static Microsoft.AspNetCore.Http.Results;

namespace TasksTracker.Tasks.Attachments.CreatingAttachment;

internal record AttachmentRequest (
    IFormFile File
);

internal static class Route {
    internal static IEndpointRouteBuilder
    UseCreateAttachmentEndpoint(this IEndpointRouteBuilder endpoints) {
        // POST api/tasks/{taskId}/attachments
        var route = endpoints.MapPost(
            pattern: "api/tasks/{taskId:guid}/attachments",
            handler: async (HttpContext context, Guid taskId) =>
            {
                if (!context.Request.HasFormContentType)
                    return BadRequest();

                var form = await context.Request.ReadFormAsync();
                var file = form.Files.FirstOrDefault();
                if (file is null || file.Length.IsZero())
                    return BadRequest();

                var fileId = Guid.NewGuid();
                var command = CreateAttachmentCommand.Create(
                    fileId: fileId,
                    taskId: taskId,
                    file: file
                );

                await context.SendCommand(command);

                return Created($"/api/tasks/{taskId}/attachments/{fileId}", fileId);
        });
        route.Accepts<AttachmentRequest>("multipart/form-data");

        return endpoints;
    }
}