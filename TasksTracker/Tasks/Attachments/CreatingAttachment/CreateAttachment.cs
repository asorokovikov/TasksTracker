using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TasksTracker.Common;
using TasksTracker.Storage;

namespace TasksTracker.Tasks.Attachments.CreatingAttachment;

public record CreateAttachmentCommand {
    public Guid FileId { get; }
    public Guid TaskId { get; }
    public IFormFile File { get; }

    [JsonConstructor]
    private CreateAttachmentCommand(Guid fileId, Guid taskId, IFormFile file) {
        FileId = fileId;
        TaskId = taskId;
        File = file;
    }

    public static CreateAttachmentCommand
    Create(Guid? fileId, Guid? taskId, IFormFile? file) => new(
        fileId: fileId.VerifyNotEmpty(nameof(fileId)),
        taskId: taskId.VerifyNotEmpty(nameof(taskId)),
        file: file.VerifyNotNull()
    );
}

internal sealed class CreateAttachmentCommandHandler : ICommandHandler<CreateAttachmentCommand> {
    private readonly Func<Attachment, CancellationToken, ValueTask> _addAttachment;
    private readonly IAttachmentClientFactory _factory;

    public CreateAttachmentCommandHandler(
        Func<Attachment, CancellationToken, ValueTask> addAttachment, 
        IAttachmentClientFactory factory
    ) {
        _addAttachment = addAttachment;
        _factory = factory;
    }

    public async ValueTask Handle(CreateAttachmentCommand command, CancellationToken ct = default) {
        var client = _factory.CreateClient(command.TaskId);
        await client.UploadAsync(command.FileId, command.File, ct);
        await _addAttachment(command.ToAttachment(), ct);
    }
}

internal static class CommandHandlerHelper {
    public static Attachment 
    ToAttachment(this CreateAttachmentCommand command) => new (
        id: command.FileId,
        taskId: command.TaskId,
        filename: command.File.FileName,
        size: command.File.Length,
        createdAt: DateTime.Now
    );
}