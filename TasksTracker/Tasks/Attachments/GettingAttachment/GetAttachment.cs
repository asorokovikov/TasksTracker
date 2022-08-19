using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TasksTracker.Common;
using TasksTracker.Storage;

namespace TasksTracker.Tasks.Attachments.GettingAttachment;

public record FileItem(string Filename, Stream Stream);

public record GetAttachmentQuery {
    public Guid TaskId { get; }
    public Guid FileId { get; }

    [JsonConstructor]
    public GetAttachmentQuery(Guid taskId, Guid fileId) {
        TaskId = taskId;
        FileId = fileId;
    }

    public static GetAttachmentQuery
    Create(Guid? taskId, Guid? fileId) => new(
        taskId: taskId.VerifyNotEmpty(nameof(taskId)),
        fileId: fileId.VerifyNotEmpty(nameof(fileId))
    );
}

internal sealed class 
GetAttachmentQueryHandler : IQueryHandler<GetAttachmentQuery, FileItem> {
    private readonly IAttachmentClientFactory _factory;
    private readonly IQueryable<Attachment> _attachments;

    public GetAttachmentQueryHandler(
        IAttachmentClientFactory factory,
        IQueryable<Attachment> attachments) {
        _factory = factory;
        _attachments = attachments;
    }

    public async ValueTask<FileItem> Handle(GetAttachmentQuery query, CancellationToken ct = default) {
        var attachment = await _attachments.SingleOrDefaultAsync(x => x.Id == query.FileId, ct);
        if (attachment == null)
            throw new KeyNotFoundException("Not found");
        var client = _factory.CreateClient(query.TaskId);
        var stream = new MemoryStream();
        await client.DownloadToAsync(stream, query.FileId, ct);
        stream.Seek(0, SeekOrigin.Begin); 
        return new (attachment.Filename, stream);
    }
}