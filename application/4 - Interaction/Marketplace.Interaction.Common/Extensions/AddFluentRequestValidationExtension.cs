using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Interaction.Common.Extensions;

public static class AddFluentRequestValidationExtension
{
    /// <summary>
    /// Регистрирует валидаторы для моделей в системе
    /// </summary>
    public static void AddFluentRequestValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(AddFluentRequestValidationExtension).Assembly);
    }
}