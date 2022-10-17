using System.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Common.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Осуществляет удаление сервиса из коллекции сервисов
    /// </summary>
    public static IServiceCollection Remove<T>(this IServiceCollection services)
    {
        if (services.IsReadOnly)
        {
            throw new ReadOnlyException($"{nameof(services)} is read only");
        }

        var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(T));
        if (serviceDescriptor == null) return services;

        services.Remove(serviceDescriptor);
        return services;
    }
}