using Newtonsoft.Json;
using TasksTracker.Common;
using TasksTracker.Storage;

namespace TasksTracker.Tasks.CreatingTask;

public record CreateTaskCommand {
    public Guid TaskId { get; }
    public string Name { get; }
    public TaskState State { get; }
    public DateTime CreatedAt { get; }

    // Private ctor can be using during the serialization.
    [JsonConstructor]
    private CreateTaskCommand(Guid taskId, string name, TaskState state, DateTime createdAt) {
        TaskId = taskId;
        Name = name;
        State = state;
        CreatedAt = createdAt;
    }

    // Splitting the validation logic and construction logic.
    // If we create objects from a request, the request fields can be null.
    public static CreateTaskCommand
    Create(Guid? id, string? name, string? state, DateTime? createdAt) => new(
        taskId: id.VerifyNotEmpty(nameof(id)),
        name: name.VerifyNotNullOrEmpty(nameof(name)).VerifyLengthLessOrEqual(255),
        state: state.VerifyType<TaskState>(nameof(state)),
        createdAt: createdAt ?? DateTime.Now
    );
}

internal sealed class CreateTaskCommandHandler : ICommandHandler<CreateTaskCommand> {
    private readonly Func<TrackerTask, CancellationToken, ValueTask> _onAddTask;

    public CreateTaskCommandHandler(Func<TrackerTask, CancellationToken, ValueTask> onAddTask) =>
        _onAddTask = onAddTask;

    public async ValueTask Handle(CreateTaskCommand command, CancellationToken ct = default) {
        var taskEntity = new TrackerTask(
            taskId: command.TaskId,
            name: command.Name,
            state: command.State,
            createdAt: command.CreatedAt,
            attachments: new ()
        );
        await _onAddTask(taskEntity, ct);
    }
}