using TasksTracker;
using TasksTracker.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddRouting();
    builder.Services.AddTasksTracker(builder.Configuration);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
{
    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseRouting();
    app.UseEndpoints(endpoints => endpoints.UseTasksTrackerEndpoints());
    if (app.Environment.IsDevelopment()) {
        app.ApplyPendingMigrations();
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}

app.Run();

// for testing purpose
public partial class Program { }