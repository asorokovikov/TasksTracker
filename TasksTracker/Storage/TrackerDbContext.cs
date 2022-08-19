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
        var connectionString = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory.AppendPath(@"../../../../TasksTracker.Api/bin/Debug/net6.0"))
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build()
            .GetConnectionString("TrackerDatabase");
        builder.UseSqlServer(connectionString);
        return new TrackerDbContext(builder.Options);
    }
}

internal static class TrackerDbContextHelper {
    public static async ValueTask 
    OnAddAndSave<T>(this DbContext dataContext, T entity, CancellationToken ct) where T : notnull {
        await dataContext.AddAsync(entity, ct);
        await dataContext.SaveChangesAsync(ct);
    }
    
    public static async ValueTask
    OnRemoveAttachment(this TrackerDbContext dataContext, Guid id, CancellationToken ct) {
        var attachment = await dataContext.Attachments.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (attachment == null)
            throw new KeyNotFoundException("Not found");
        dataContext.Attachments.Remove(attachment);
        await dataContext.SaveChangesAsync(ct);
    }
}