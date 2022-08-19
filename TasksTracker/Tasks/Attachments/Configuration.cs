using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TasksTracker.Common;
using TasksTracker.Storage;
using TasksTracker.Tasks.Attachments.CreatingAttachment;
using TasksTracker.Tasks.Attachments.GettingAttachment;
using TasksTracker.Tasks.Attachments.GettingAttachmentsList;
using TasksTracker.Tasks.Attachments.RemovingAttachment;

namespace TasksTracker.Tasks.Attachments;

internal static class Configuration {
    
    public static IEndpointRouteBuilder
    UseAttachmentEndpoints(this IEndpointRouteBuilder endpoints) => endpoints
        .UseCreateAttachmentEndpoint()
        .UseRemoveAttachmentEndpoint()
        .UseGetAttachmentsListEndpoint()
        .UseGetAttachmentEndpoint();
    
    public static void AddAttachmentServices(this IServiceCollection services) {
        services.AddCommandHandlers();
        services.AddQueryHandlers();
    }
    
    private static void AddCommandHandlers(this IServiceCollection services) {
        services.AddCommandHandler<CreateAttachmentCommand, CreateAttachmentCommandHandler>(provider => {
            var dataContext = provider.GetRequiredService<TrackerDbContext>();
            var factory = provider.GetRequiredService<IAttachmentClientFactory>();
            return new CreateAttachmentCommandHandler(dataContext.OnAddAndSave, factory);
        });
        
        services.AddCommandHandler<RemoveAttachmentCommand, RemoveAttachmentCommandHandler>(provider => {
            var dataContext = provider.GetRequiredService<TrackerDbContext>();
            var factory = provider.GetRequiredService<IAttachmentClientFactory>();
            return new RemoveAttachmentCommandHandler(dataContext.OnRemoveAttachment, factory);
        });
    }

    private static void AddQueryHandlers(this IServiceCollection services) {
        services.AddQueryHandler<GetAttachmentsListQuery, IReadOnlyList<AttachmentItem>, GetAttachmentsListQueryHandler>(provider => {
           var dataContext = provider.GetRequiredService<TrackerDbContext>();
           return new GetAttachmentsListQueryHandler(dataContext.Set<Attachment>().AsNoTracking());
        });
        
        services.AddQueryHandler<GetAttachmentQuery, FileItem, GetAttachmentQueryHandler>(provider => {
           var dataContext = provider.GetRequiredService<TrackerDbContext>();            
           var factory = provider.GetRequiredService<IAttachmentClientFactory>();
           return new GetAttachmentQueryHandler(factory, dataContext.Set<Attachment>().AsNoTracking());
        });
    }
}