using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Interaction.Common.Extensions;

public static class AddAutoMappingExtension
{
    /// <summary>
    /// Добавляет автоматический маппер в зависимости
    /// </summary>
    public static IServiceCollection AddAutoMapping(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AddAutoMappingExtension).Assembly);
        return services;
    }
}