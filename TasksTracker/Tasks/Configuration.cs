using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TasksTracker.Common;
using TasksTracker.Storage;
using TasksTracker.Tasks.CreatingTask;
using TasksTracker.Tasks.GettingTask;
using TasksTracker.Tasks.GettingTasksList;
using TasksTracker.Tasks.RemovingTask;
using TasksTracker.Tasks.UpdatingTask;

namespace TasksTracker.Tasks;

internal static class Configuration {

    public static IEndpointRouteBuilder
    UseTaskEndpoints(this IEndpointRouteBuilder endpoints) => endpoints
        .UseCreateTaskEndpoint()
        .UseRemoveTaskEndpoint()
        .UseUpdateTaskEndpoint()
        .UseGetTaskEndpoint()
        .UseGetTasksListEndpoint();

    public static void AddTaskServices(this IServiceCollection services) {
        services.AddCommandHandlers();
        services.AddQueryHandlers();
    }

    private static void AddCommandHandlers(this IServiceCollection services) {
        services.AddCommandHandler<RemoveTaskCommand, RemoveTaskCommandHandler>();
        services.AddCommandHandler<UpdateTaskCommand, UpdateTaskCommandHandler>();
        services.AddCommandHandler<CreateTaskCommand, CreateTaskCommandHandler>(provider => {
            var dataContext = provider.GetRequiredService<TrackerDbContext>();
            return new CreateTaskCommandHandler(dataContext.OnAddAndSave);
        });
    }

    private static void AddQueryHandlers(this IServiceCollection services) {
        services.AddQueryHandler<GetTaskQuery, TaskItem?, GetTaskQueryHandler>(provider => {
            var dataContext = provider.GetRequiredService<TrackerDbContext>();
            return new GetTaskQueryHandler(dataContext.Set<TrackerTask>().AsNoTracking());
        });
        services.AddQueryHandler<GetTasksListQuery, IReadOnlyList<TaskItem>, GetTasksListQueryHandler>(provider => {
            var db = provider.GetRequiredService<TrackerDbContext>();
            return new GetTasksListQueryHandler(db.Set<TrackerTask>().AsNoTracking());
        });
    }
}