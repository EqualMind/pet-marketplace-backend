using Marketplace.Common.ExceptionHandling;

namespace Marketplace.Common.Architecture;

/// <summary>
/// Общий интерфейс для всех обработчиков событий
/// </summary>
public interface IApplicationEventHandler
{
    /// <summary>
    /// Осуществляет алгоритм обработки события
    /// </summary>
    /// <param name="event">Объект вызванного события</param>
    Task HandleAsync(object @event);
}

/// <summary>
/// Базовый класс для обработчиков событий
/// </summary>
/// <typeparam name="TEvent">Тип обрабатываемого события</typeparam>
public abstract class ApplicationEventHandler<TEvent> : IApplicationEventHandler where TEvent : class, IApplicationEvent
{
    public async Task HandleAsync(object @event)
    {
        if (@event is not TEvent applicationEvent) 
            throw new InternalServerErrorException($"Wrong event type! Expected {typeof(TEvent).FullName}, but actual is {@event.GetType().FullName}");

        await HandleAsync(applicationEvent);
    }
    
    protected abstract Task HandleAsync(TEvent @event);
}

/// <summary>
/// Специальный адаптер для функциональных обработчиков системных событий
/// </summary>
/// <typeparam name="TEvent">Тип события</typeparam>
internal sealed class FunctionalEventHandler<TEvent> : ApplicationEventHandler<TEvent> where TEvent : class, IApplicationEvent
{
    private readonly IServiceProvider serviceProvider;
    private readonly Func<TEvent, IServiceProvider, Task> eventHandlerFunc;

    public FunctionalEventHandler(IServiceProvider serviceProvider, Func<TEvent, IServiceProvider, Task> eventHandlerFunc)
    {
        this.serviceProvider = serviceProvider;
        this.eventHandlerFunc = eventHandlerFunc;
    }

    protected override async Task HandleAsync(TEvent @event) => await eventHandlerFunc(@event, serviceProvider);
}