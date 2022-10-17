using System.Reflection;
using Marketplace.Common.Architecture;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Common.Extensions;

public static class AddOperationsExtension
{
    /// <summary>
    /// Осуществляет автоматический поиск и регистрацию бизнес-операций в системе
    /// </summary>
    public static IServiceCollection AddOperations(this IServiceCollection services, params Assembly[] assemblies)
    {
        var operations = assemblies
            .SelectMany(x => x.GetTypes())
            .Where(t => t.IsAssignableTo(typeof(IOperation)) && t.IsClass)
            .ToList();
        
        operations.ForEach(operation => services.AddTransient(operation));
        return services;
    }
}