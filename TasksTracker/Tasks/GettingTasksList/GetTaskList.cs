using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TasksTracker.Common;
using TasksTracker.Storage;

namespace TasksTracker.Tasks.GettingTasksList;

public record GetTasksListQuery {
    private const int DefaultPage = 1;
    private const int DefaultPageSize = 10;

    public int Page { get; }
    public int PageSize { get; }
    public string? Filter { get; }

    [JsonConstructor]
    public GetTasksListQuery(int page, int pageSize, string? filter) {
        Page = page;
        PageSize = pageSize;
        Filter = filter;
    }

    public static GetTasksListQuery
    Create(int? page, int? pageSize, string? filter) => new (
        page: (page ?? DefaultPage).VerifyGreaterZero(nameof(page)), 
        pageSize: (pageSize ?? DefaultPageSize).VerifyGreaterZero(nameof(pageSize)), 
        filter: filter
    );
}

internal sealed class 
GetTasksListQueryHandler : IQueryHandler<GetTasksListQuery, IReadOnlyList<TaskItem>> {
    private readonly IQueryable<TrackerTask> _tasks;

    public GetTasksListQueryHandler(IQueryable<TrackerTask> tasks) => _tasks = tasks;

    public async ValueTask<IReadOnlyList<TaskItem>> 
    Handle(GetTasksListQuery query, CancellationToken ct = default) {
        var tasks = _tasks;
        if (query.Filter != null)
            tasks = tasks.Where(x => x.Name.Contains(query.Filter));
        return await tasks.OrderBy(x => x.CreatedAt)
            .Skip(query.PageSize * (query.Page - 1))
            .Take((query.PageSize))
            .Select(x => x.ToTaskItem())
            .ToListAsync(ct);
    }
}