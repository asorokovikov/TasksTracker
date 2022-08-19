using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using TasksTracker.Common;
using TasksTracker.Storage;

namespace TasksTracker.Tests;

public sealed class TrackerWebApplicationFactory : WebApplicationFactory<Program> {
    private readonly string _databaseName = Guid.NewGuid().ToString("N").ToLower();
    public string ContentRootDirectory => "Attachments" + _databaseName;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        builder.ConfigureServices(services => {
            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            services.Configure<AttachmentConfiguration>(x => {
                x.ContentRootDirectory += _databaseName;
            });
            
            var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<TrackerDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);
            services.AddDbContext<TrackerDbContext>(options => options.UseInMemoryDatabase(_databaseName));
            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            using var dataContext = scope.ServiceProvider.GetRequiredService<TrackerDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<TrackerWebApplicationFactory>>();
            try {
                dataContext.Database.EnsureCreated();
                dataContext.SaveChanges();
            }
            catch (Exception exception) {
                logger.LogError(exception, "An error occured seeding " +
                    "the database with test data: {Error}", exception.Message);
            }
        });
    }

    protected override void Dispose(bool disposing) {
        if (disposing) 
            ContentRootDirectory.DeleteDirectoryIfExists();
        base.Dispose(disposing);
    }
}