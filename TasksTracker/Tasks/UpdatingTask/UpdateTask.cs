using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TasksTracker.Common;
using TasksTracker.Storage;

namespace TasksTracker.Tasks.UpdatingTask;

public record UpdateTaskCommand {
    public Guid TaskId { get; }
    public string Name { get; }
    public TaskState State { get; }
    public DateTime CreatedAt { get; }

    // Private ctor can be using during the serialization.
    [JsonConstructor]
    private UpdateTaskCommand(Guid taskId, string name, TaskState state, DateTime createdAt) {
        TaskId = taskId;
        Name = name;
        State = state;
        CreatedAt = createdAt;
    }

    // Splitting the validation logic and construction logic.
    // If we create objects from a request, the request fields can be null.
    public static UpdateTaskCommand
    Create(Guid? id, string? name, string? state, DateTime? createdAt) => new(
        taskId: id.VerifyNotEmpty(nameof(id)),
        name: name.VerifyNotNullOrEmpty(nameof(name)),
        state: state.VerifyType<TaskState>(nameof(state)),
        createdAt: createdAt ?? DateTime.Now
    );
}

internal sealed class
UpdateTaskCommandHandler : ICommandHandler<UpdateTaskCommand> {
    private readonly TrackerDbContext _dataContext;

    public UpdateTaskCommandHandler(TrackerDbContext dataContext) => _dataContext = dataContext;

    public async ValueTask Handle(UpdateTaskCommand command, CancellationToken ct = default) {
        var item = await _dataContext.Tasks.AsNoTracking().SingleOrDefaultAsync(x => x.TaskId == command.TaskId, ct);
        if (item == null)
            throw new KeyNotFoundException("Not found");
        var updatedItem = new TrackerTask(
            taskId: item.TaskId,
            name: command.Name,
            state: command.State,
            createdAt: command.CreatedAt,
            attachments: item.Attachments
        );
        _dataContext.Tasks.Update(updatedItem);
        await _dataContext.SaveChangesAsync(ct);
    }
}