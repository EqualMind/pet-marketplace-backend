namespace Marketplace.Common.Architecture;

/// <summary>
/// Интерфейс для инъекции зависимостей и работы с контейнером.
/// Описывает интерфейс для взаимодействия с системой событий
/// </summary>
public interface IApplicationEventConfiguration
{
    internal Type EventType { get; }
    internal List<ApplicationEventHandlerDescription> HandlerDescriptions { get; }
}

/// <summary>
/// Конфигурация системного события
/// </summary>
/// <typeparam name="TEvent">Тип события</typeparam>
public abstract class ApplicationEventConfiguration<TEvent> : IApplicationEventConfiguration where TEvent : class, IApplicationEvent
{
    private readonly List<ApplicationEventHandlerDescription<TEvent>> handlerTypes = new();

    Type IApplicationEventConfiguration.EventType => typeof(TEvent);
    List<ApplicationEventHandlerDescription> IApplicationEventConfiguration.HandlerDescriptions => handlerTypes.Cast<ApplicationEventHandlerDescription>().ToList();

    /// <summary>
    /// Регистрирует обработчик события
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика</typeparam>
    protected void Bind<THandler>() where THandler : ApplicationEventHandler<TEvent>, IApplicationEventHandler => handlerTypes.Add(new()
    {
        HandlerType = typeof(THandler)
    });
    
    /// <summary>
    /// Регистрирует обработчик события в функциональном стиле
    /// </summary>
    /// <param name="handlerFunc">Функциональный обработчик события</param>
    protected void Bind(Func<TEvent, IServiceProvider, Task> handlerFunc) => handlerTypes.Add(new()
    {
        HandlerType = typeof(FunctionalEventHandler<TEvent>),
        HandlerFunc = handlerFunc
    });
}