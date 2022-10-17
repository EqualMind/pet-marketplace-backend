using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Common.Architecture;

/// <summary>
/// Описание обработчика события приложения
/// </summary>
internal abstract class ApplicationEventHandlerDescription
{
    public Type HandlerType { get; init; }
    public abstract void RegisterAsService(IServiceCollection services);
}

/// <summary>
/// Описание обработчика события приложения с указанием типа события
/// </summary>
internal sealed class ApplicationEventHandlerDescription<TEvent> : ApplicationEventHandlerDescription where TEvent : class, IApplicationEvent
{
    public bool IsFunctionalHandler => HandlerFunc != null;
    public Func<TEvent, IServiceProvider, Task>? HandlerFunc { get; init; }
    
    public override void RegisterAsService(IServiceCollection services)
    {
        if (IsFunctionalHandler)
        {
            services.AddTransient(HandlerType, provider => ActivatorUtilities.CreateInstance(provider, HandlerType, HandlerFunc!));
        }
        else
        {
            services.AddTransient(HandlerType);
        }
    }
}