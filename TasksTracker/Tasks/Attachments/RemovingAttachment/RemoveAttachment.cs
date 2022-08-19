using Newtonsoft.Json;
using TasksTracker.Common;
using TasksTracker.Storage;

namespace TasksTracker.Tasks.Attachments.RemovingAttachment;

public record RemoveAttachmentCommand {
    public Guid FileId { get; }
    public Guid TaskId { get; }

    [JsonConstructor]
    private RemoveAttachmentCommand(Guid fileId, Guid taskId) {
        FileId = fileId;
        TaskId = taskId;
    }

    public static RemoveAttachmentCommand
    Create(Guid? fileId, Guid? taskId) => new(
        fileId: fileId.VerifyNotEmpty(nameof(fileId)),
        taskId: taskId.VerifyNotEmpty(nameof(taskId))
    );
}

internal sealed class RemoveAttachmentCommandHandler : ICommandHandler<RemoveAttachmentCommand> {
    private readonly Func<Guid, CancellationToken, ValueTask> _removeAttachment;
    private readonly IAttachmentClientFactory _factory;

    public RemoveAttachmentCommandHandler(
        Func<Guid, CancellationToken, ValueTask> removeAttachment,
        IAttachmentClientFactory factory
    ) {
        _removeAttachment = removeAttachment;
        _factory = factory;
    }

    public async ValueTask Handle(RemoveAttachmentCommand command, CancellationToken ct = default) {
        await _removeAttachment(command.FileId, ct);
        var client = _factory.CreateClient(command.TaskId);
        await client.DeleteAsync(command.FileId, ct);
    }
}