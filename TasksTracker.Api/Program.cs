using TasksTracker;
using TasksTracker.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);
{
    var fileSizeLimit = builder.Configuration.GetValue<long>("AttachmentConfiguration:FileSizeLimit");
    builder.WebHost.ConfigureKestrel((_, options) => options.Limits.MaxRequestBodySize = fileSizeLimit);

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
    app.ApplyPendingMigrations();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

// for testing purpose
public partial class Program { }