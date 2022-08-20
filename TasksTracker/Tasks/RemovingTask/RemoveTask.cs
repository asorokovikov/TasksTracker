using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TasksTracker.Common;
using TasksTracker.Storage;

namespace TasksTracker.Tasks.RemovingTask;

public record RemoveTaskCommand {
    public Guid TaskId { get; }

    [JsonConstructor]
    private RemoveTaskCommand(Guid taskId) => TaskId = taskId;

    public static RemoveTaskCommand
    Create(Guid? id) => new(
        taskId: id.VerifyNotEmpty(nameof(id))
    );
}

internal sealed class
RemoveTaskCommandHandler : ICommandHandler<RemoveTaskCommand> {
    private readonly TrackerDbContext _dataContext;

    public RemoveTaskCommandHandler(TrackerDbContext dataContext) =>
        _dataContext = dataContext;

    public async ValueTask
    Handle(RemoveTaskCommand command, CancellationToken ct = default) {
        var item = await _dataContext.Tasks.FirstOrDefaultAsync(x => x.TaskId == command.TaskId, ct);
        if (item == null)
            throw new KeyNotFoundException("Not found");
        _dataContext.Tasks.Remove(item);
        await _dataContext.SaveChangesAsync(ct);
    }
}