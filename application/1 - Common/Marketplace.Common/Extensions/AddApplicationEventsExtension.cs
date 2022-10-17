using System.Reflection;
using Marketplace.Common.Architecture;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Marketplace.Common.Extensions;

public static class AddApplicationEventsExtension
{
    /// <summary>
    /// Активирует работу с системой внутренних событий приложения
    /// </summary>
    public static IServiceCollection AddApplicationEvents(this IServiceCollection services, params Assembly[] assemblies)
    {
        var provider = services.BuildServiceProvider();
        var registry = provider.GetService<ApplicationEventHandlersRegistry>() ?? new ApplicationEventHandlersRegistry();

        var eventConfigurations = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsAssignableTo(typeof(IApplicationEventConfiguration)))
            .Where(type => type.IsClass)
            .Where(type => type.GetConstructor(Type.EmptyTypes) != null)
            .Select(type => (IApplicationEventConfiguration)Activator.CreateInstance(type)!)
            .ToList();

        foreach (var eventConfig in eventConfigurations)
        {
            eventConfig.HandlerDescriptions.ForEach(x => x.RegisterAsService(services));
            registry.ApplyConfiguration(eventConfig);
        }

        services.Replace(new(typeof(ApplicationEventHandlersRegistry), registry));
        services.Replace(new(typeof(ApplicationEventBus), typeof(ApplicationEventBus), ServiceLifetime.Scoped));
        
        return services;
    }
}