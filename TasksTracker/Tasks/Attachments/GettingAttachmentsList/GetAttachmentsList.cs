using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TasksTracker.Common;
using TasksTracker.Storage;

namespace TasksTracker.Tasks.Attachments.GettingAttachmentsList;

public record AttachmentItem(
    Guid Id,
    string Filename,
    long Size
);

public record GetAttachmentsListQuery {
    public Guid TaskId { get; }

    [JsonConstructor]
    private GetAttachmentsListQuery(Guid taskId) => TaskId = taskId;

    public static GetAttachmentsListQuery
    Create(Guid? taskId) => new(
        taskId: taskId.VerifyNotEmpty(nameof(taskId))
    );
}

internal sealed class 
GetAttachmentsListQueryHandler : IQueryHandler<GetAttachmentsListQuery, IReadOnlyList<AttachmentItem>> {
    private readonly IQueryable<Attachment> _attachments;

    public GetAttachmentsListQueryHandler(IQueryable<Attachment> attachments) => 
        _attachments = attachments;

    public async ValueTask<IReadOnlyList<AttachmentItem>> 
    Handle(GetAttachmentsListQuery query, CancellationToken ct = default) => await _attachments            
        .Where(x => x.TaskId == query.TaskId)
        .Select(x => x.ToAttachmentItem())
        .ToListAsync(ct);
}

internal static class AttachmentHelper {
    public static AttachmentItem 
    ToAttachmentItem(this Attachment attachment) => new (
        Id: attachment.Id,
        Filename: attachment.Filename,
        Size: attachment.Size
    );
}