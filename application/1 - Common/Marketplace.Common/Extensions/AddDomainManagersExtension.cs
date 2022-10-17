using System.Linq.Expressions;
using System.Reflection;
using LinqKit;
using Marketplace.Common.Architecture;
using Marketplace.Common.Guards;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Common.Extensions;

public static class AddDomainManagersExtension
{
    /// <summary>
    /// Осуществляет поиск и регистрацию доменных сервисов в сборке
    /// </summary>
    public static void AddDomainManagers(this IServiceCollection services, Assembly assembly)
    {
        var domainManagers = assembly.GetTypes()
            .Where(IsDomainManager.Compile())
            .ToList();

        foreach (var domainManager in domainManagers)
        {
            services.AddScoped(domainManager, provider =>
            {
                var instance = ActivatorUtilities.CreateInstance(provider, domainManager) as DomainManager;
                
                Guard.Object.NotNull(instance, $"Can't create {domainManager.Name}({nameof(DomainManager)}) instance!");
                
                instance!.RegisterStorage(provider.GetRequiredService<IApplicationStorage>());
                return instance;
            });
        }
    }

    private static Expression<Func<Type, bool>> IsDomainManager
        => PredicateBuilder.New<Type>(true)
            .And(t => t.IsClass)
            .And(t => !t.IsAbstract)
            .And(t => t.IsSubclassOf(typeof(DomainManager)));
}