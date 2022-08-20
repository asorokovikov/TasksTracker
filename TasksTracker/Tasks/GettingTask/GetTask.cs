using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TasksTracker.Common;
using TasksTracker.Storage;

namespace TasksTracker.Tasks.GettingTask;

public record GetTaskQuery {
    public Guid TaskId { get; }

    [JsonConstructor]
    private GetTaskQuery(Guid taskId) => TaskId = taskId;

    public static GetTaskQuery
    Create(Guid? id) => new(taskId: id.VerifyNotEmpty(nameof(id)));
}

internal sealed class
GetTaskQueryHandler : IQueryHandler<GetTaskQuery, TaskItem?> {
    private readonly IQueryable<TrackerTask> _tasks;

    public GetTaskQueryHandler(IQueryable<TrackerTask> tasks) => _tasks = tasks;

    public async ValueTask<TaskItem?> Handle(GetTaskQuery query, CancellationToken ct = default) {
        var task = await _tasks.SingleOrDefaultAsync(x => x.TaskId == query.TaskId, ct);
        return task != null
            ? new TaskItem(task.TaskId, task.Name, task.State.ToString(), task.CreatedAt)
            : null;
    }
}