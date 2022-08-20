
namespace TasksTracker.Tasks;

public enum TaskState {
    New,
    InProgress,
    Done
}

public record TaskItem(
    Guid TaskId,
    string Name,
    string State,
    DateTime CreatedAt
);