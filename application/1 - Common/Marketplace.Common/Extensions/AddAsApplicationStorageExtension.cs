using Marketplace.Common.Architecture;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Common.Extensions;

public static class AddAsApplicationStorageExtension
{
    /// <summary>
    /// Регистрирует реализацию <see cref="IApplicationStorage"/>
    /// </summary>
    public static void AddAsApplicationStorageImplementation<TImplementation>(this IServiceCollection services)
        where TImplementation : IApplicationStorage
    {
        services.AddScoped<IApplicationStorage>(provider => provider.GetRequiredService<TImplementation>());
        services.AddScoped<IApplicationStorageReader>(provider => provider.GetRequiredService<TImplementation>());
    }
}