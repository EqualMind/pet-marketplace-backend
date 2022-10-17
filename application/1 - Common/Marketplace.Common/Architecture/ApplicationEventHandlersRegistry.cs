namespace Marketplace.Common.Architecture;

/// <summary>
/// Реестр обработчиков событий
/// </summary>
public sealed class ApplicationEventHandlersRegistry
{
    private readonly Dictionary<Type, List<Type>> map = new();

    /// <summary>
    /// Регистрирует событие и его настройку в реестре обработчиков событий
    /// </summary>
    /// <param name="configuration">Настройки события</param>
    public void ApplyConfiguration(IApplicationEventConfiguration configuration) 
        => map.Add(configuration.EventType, configuration.HandlerDescriptions.Select(x => x.HandlerType).ToList());

    /// <summary>
    /// Возвращает список типов обработчиков событий зарегистрированных за указанным типом события
    /// </summary>
    /// <param name="eventType">Тип события</param>
    public List<Type> GetEventHandlersTypes(Type eventType) => map.GetValueOrDefault(eventType, new());
}