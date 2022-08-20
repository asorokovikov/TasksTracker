using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TasksTracker.Tasks;

namespace TasksTracker.Storage;

internal sealed class TrackerTask {
    public Guid TaskId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public TaskState State { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public List<Attachment> Attachments { get; private set; } = new ();

    private TrackerTask() { }
    public TrackerTask(Guid taskId, string name, TaskState state, DateTime createdAt, List<Attachment> attachments) {
        TaskId = taskId;
        Name = name;
        State = state;
        CreatedAt = createdAt;
        Attachments = attachments;
    }

    public static void Configure(EntityTypeBuilder<TrackerTask> builder) {
        builder.HasKey(x => x.TaskId);
        builder.HasIndex(x => x.CreatedAt);
        builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
        builder.Property(x => x.State).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.HasMany(x => x.Attachments).WithOne().HasForeignKey(x => x.TaskId);
        builder.HasData(EntitiesHelper.GetTasksSeedingData());
    }
}

internal sealed class Attachment {
    public Guid Id { get; private set; }
    public Guid TaskId { get; private set; }
    public string Filename { get; private set; }
    public long Size { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Attachment(Guid id, Guid taskId, string filename, long size, DateTime createdAt) {
        Id = id;
        TaskId = taskId;
        Filename = filename;
        Size = size;
        CreatedAt = createdAt;
    }
    public static void Configure(EntityTypeBuilder<Attachment> builder) {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Filename).HasMaxLength(255).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
    }
}

internal static class EntitiesHelper {
    public static TaskItem
    ToTaskItem(this TrackerTask model) => new(
        TaskId: model.TaskId,
        Name: model.Name,
        State: model.State.ToString(),
        CreatedAt: model.CreatedAt
    );

    public static IEnumerable<TrackerTask>
    GetTasksSeedingData(int count = 100) {
        for (var index = 0; index < count; index++) {
            yield return new(
                taskId: Guid.NewGuid(),
                name: $"Task #{index + 1}",
                state: TaskState.New,
                createdAt: DateTime.Now,
                attachments: new()
            );
        }
    }
}