using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TasksTracker.Common;

namespace TasksTracker.Storage;

internal sealed class TrackerDbContext : DbContext {
    public DbSet<TrackerTask> Tasks => Set<TrackerTask>();
    public DbSet<Attachment> Attachments => Set<Attachment>();

    public TrackerDbContext(DbContextOptions<TrackerDbContext> options) : base(options) {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<TrackerTask>(TrackerTask.Configure);
        modelBuilder.Entity<Attachment>(Attachment.Configure);
    }
}

internal sealed class TrackerDbContextFactory : IDesignTimeDbContextFactory<TrackerDbContext> {
    public TrackerDbContext CreateDbContext(string[] args) {
        var builder = new DbContextOptionsBuilder<TrackerDbContext>();
        if (builder.IsConfigured)
            return new TrackerDbContext(builder.Options);
        var environmentName = Environment.GetEnvironmentVariable("EnvironmentName") ?? "Development";
        var connectionString = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory.AppendPath("../../../../TasksTracker.Api/bin/Debug/net6.0"))
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build()
            .GetConnectionString("TrackerDatabase");
        builder.UseSqlServer(connectionString);
        return new TrackerDbContext(builder.Options);
    }
}

internal static class TrackerDbContextHelper {
    public static async ValueTask
    OnAddAndSave<T>(this DbContext context, T entity, CancellationToken ct) where T : notnull {
        await context.AddAsync(entity, ct);
        await context.SaveChangesAsync(ct);
    }

    public static async ValueTask
    OnRemoveAttachment(this TrackerDbContext context, Guid id, CancellationToken ct) {
        var attachment = await context.Attachments.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (attachment == null)
            throw new KeyNotFoundException("Not found");
        context.Attachments.Remove(attachment);
        await context.SaveChangesAsync(ct);
    }
}