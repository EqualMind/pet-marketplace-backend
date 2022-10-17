using FluentValidation;
using Marketplace.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.ApplicationCore.Domain;

public static class AddDomainServicesExtension
{
    /// <summary>
    /// Осуществляет регистрацию доменных сервисов
    /// </summary>
    public static void AddDomainServices(this IServiceCollection services)
    {
        var assembly = typeof(AddDomainServicesExtension).Assembly;
        
        services.AddDomainManagers(assembly);
        services.AddValidatorsFromAssembly(assembly);
    }
}