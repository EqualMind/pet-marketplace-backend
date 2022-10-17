using FluentValidation;
using Marketplace.ApplicationCore.Domain;
using Marketplace.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.ApplicationCore.Operations;

public static class AddUseCasesExtension
{
    /// <summary>
    /// Подключает бизнес-правила приложения в проект
    /// </summary>
    public static void AddUseCases(this IServiceCollection services)
    {
        var assembly = typeof(AddUseCasesExtension).Assembly;
        
        services.AddDomainServices();
        services.AddValidatorsFromAssembly(assembly);
        services.AddApplicationEvents(assembly);
        services.AddOperations(assembly);
    }
}