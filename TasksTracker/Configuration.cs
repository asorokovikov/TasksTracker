using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TasksTracker.Storage;
using TasksTracker.Tasks;
using TasksTracker.Tasks.Attachments;

namespace TasksTracker;

public static class Configuration {
    public static IServiceCollection
    AddTasksTracker(this IServiceCollection services, ConfigurationManager configuration) {
        services.Configure<AttachmentConfiguration>(configuration.GetSection(nameof(AttachmentConfiguration)));
        services.AddTransient<IAttachmentClientFactory, LocalAttachmentClientFactory>();
        services.AddDbContext<TrackerDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("TrackerDatabase")));
        services.AddTaskServices();
        services.AddAttachmentServices();
        return services;
    }

    public static IEndpointRouteBuilder
    UseTasksTrackerEndpoints(this IEndpointRouteBuilder endpoints) => endpoints
        .UseTaskEndpoints()
        .UseAttachmentEndpoints();

    public static IApplicationBuilder
    ApplyPendingMigrations(this IApplicationBuilder app) {
        using var scope = app.ApplicationServices.CreateScope();
        using var dataContext = scope.ServiceProvider.GetRequiredService<TrackerDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<TrackerDbContext>>();

        if (!dataContext.Database.IsSqlServer())
            return app;

        try {
            logger.LogInformation("Migrating dataase {Database}...", nameof(TrackerDbContext));
            dataContext.Database.Migrate();
        }
        catch (Exception ex) {
            logger.LogError(ex, "An error occurred while migrating the database: {Error}", ex.Message);
            throw;
        }
        return app;
    }
}