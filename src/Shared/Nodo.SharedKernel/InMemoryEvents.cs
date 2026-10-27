using System.Reflection;
using System.Runtime.ExceptionServices;
using Microsoft.Extensions.DependencyInjection;

namespace Nodo.SharedKernel;

internal static class HandlerInvoker
{
    public static async Task InvokeAsync(IServiceProvider sp, Type openHandlerType, object @event, CancellationToken ct)
    {
        var handlerType = openHandlerType.MakeGenericType(@event.GetType());
        var method = handlerType.GetMethod("HandleAsync")!;
        foreach (var handler in sp.GetServices(handlerType))
        {
            try
            {
                var result = method.Invoke(handler, [@event, ct])!;
                await (Task)result;
            }
            catch (TargetInvocationException ex) when (ex.InnerException is not null)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
        }
    }
}

internal sealed class InMemoryDomainEventDispatcher(IServiceProvider sp) : IDomainEventDispatcher
{
    public Task DispatchAsync(IDomainEvent domainEvent, CancellationToken ct = default) =>
        HandlerInvoker.InvokeAsync(sp, typeof(IDomainEventHandler<>), domainEvent, ct);
}

internal sealed class InMemoryEventBus(IServiceProvider sp) : IEventBus
{
    public Task PublishAsync<T>(T integrationEvent, CancellationToken ct = default) where T : IIntegrationEvent =>
        HandlerInvoker.InvokeAsync(sp, typeof(IIntegrationEventHandler<>), integrationEvent!, ct);
}

public static class SharedKernelModule
{
    public static IServiceCollection AddSharedKernel(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, InMemoryDomainEventDispatcher>();
        services.AddScoped<IEventBus, InMemoryEventBus>();
        return services;
    }
}
