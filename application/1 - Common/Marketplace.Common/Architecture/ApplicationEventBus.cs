using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Common.Architecture;

public sealed class ApplicationEventBus
{
    private readonly IServiceProvider provider;
    private readonly ApplicationEventHandlersRegistry registry;

    public ApplicationEventBus(IServiceProvider provider, ApplicationEventHandlersRegistry registry)
    {
        this.provider = provider;
        this.registry = registry;
    }
    
    /// <summary>
    /// Выполняет отправку события на обработку
    /// </summary>
    /// <param name="event">Объект события</param>
    /// <typeparam name="TEvent">Тип объекта события</typeparam>
    public async Task EmitAsync<TEvent>(TEvent @event) where TEvent : class, IApplicationEvent
    {
        var handlers = registry
            .GetEventHandlersTypes(typeof(TEvent))
            .Select(handlerType => provider.GetRequiredService(handlerType) as IApplicationEventHandler)
            .ToList();

        foreach (var handler in handlers)
        {
            await handler!.HandleAsync(@event);
        }
    }
}