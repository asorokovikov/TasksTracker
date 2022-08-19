using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace TasksTracker.Common;

public interface ICommand {}

public interface ICommandHandler<in TCommand> {
    ValueTask Handle(TCommand command, CancellationToken ct = default);
}

public interface ICommandHandler<in TCommand, TResult> {
    ValueTask<TResult> Handle(TCommand command, CancellationToken ct = default);
}

public static class CommandHandlerHelper {
    public static IServiceCollection AddCommandHandler<TCommand, TCommandHandler>(
        this IServiceCollection services,
        Func<IServiceProvider, TCommandHandler>? configure = null
    ) where TCommandHandler: class, ICommandHandler<TCommand>
    {
        if (configure == null) {
            services.AddTransient<TCommandHandler, TCommandHandler>();
            return services.AddTransient<ICommandHandler<TCommand>, TCommandHandler>();
        }
        services.AddTransient<TCommandHandler, TCommandHandler>(configure);
        return services.AddTransient<ICommandHandler<TCommand>, TCommandHandler>(configure);
    }

    public static IServiceCollection AddCommandHandler<TCommand, TResult, TCommandHandler>(
        this IServiceCollection services,
        Func<IServiceProvider, TCommandHandler>? configure = null
    ) where TCommandHandler : class, ICommandHandler<TCommand, TResult> {
        if (configure == null) {
            services.AddTransient<TCommandHandler, TCommandHandler>();
            return services.AddTransient<ICommandHandler<TCommand, TResult>, TCommandHandler>();
        }
        services.AddTransient<TCommandHandler, TCommandHandler>(configure);
        return services.AddTransient<ICommandHandler<TCommand, TResult>, TCommandHandler>(configure);
    }

    public static ICommandHandler<T> GetCommandHandler<T>(this IServiceProvider provider) => 
        provider.GetRequiredService<ICommandHandler<T>>();

    public static ICommandHandler<TCommand, TResult> 
    GetCommandHandler<TCommand, TResult>(this IServiceProvider provider) => 
        provider.GetRequiredService<ICommandHandler<TCommand, TResult>>();

    public static ValueTask 
    SendCommand<T>(this HttpContext context, T command) => 
        context.RequestServices.GetCommandHandler<T>().Handle(command, context.RequestAborted);

    public static ValueTask<TResult>
    SendCommand<TCommand, TResult>(this HttpContext context, TCommand command) =>
        context.RequestServices.GetCommandHandler<TCommand, TResult>().Handle(command, context.RequestAborted);

    public static async ValueTask SendCommand<TCommand>(
        this IServiceProvider provider,
        TCommand command,
        CancellationToken ct = default) where TCommand : ICommand
    {
        if (typeof(TCommand) == typeof(ICommand))
            await provider.DispatchDynamicallyAsync(command, ct);
        else
            await provider.GetCommandHandler<TCommand>().Handle(command, ct);
    }

    private static async ValueTask
    DispatchDynamicallyAsync(this IServiceProvider provider, ICommand command, CancellationToken ct = default) {
        using var scope = provider.CreateScope();
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);
        var method = handlerType.GetMethod(nameof(ICommandHandler<ICommand>.Handle));
        if (method is null)
            throw new InvalidOperationException($"Command handler for '{command.GetType().Name}' is invalid.");
        await (ValueTask) method.Invoke(handler, new object[] { command, ct })!;
    }
}