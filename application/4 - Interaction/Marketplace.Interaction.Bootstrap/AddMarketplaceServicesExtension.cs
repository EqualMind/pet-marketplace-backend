using System.Reflection;
using Marketplace.ApplicationCore.Operations;
using Marketplace.Infrastructure.Database;
using Marketplace.Infrastructure.Services;
using Marketplace.Interaction.Common;
using Marketplace.Interaction.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Interaction.Bootstrap;

public static class AddMarketplaceServicesExtension
{
    /// <summary>
    /// Осуществляет подключение всех сервисов, необходимых для работы торговой площадки
    /// </summary>
    public static void AddMarketplaceServices(this IServiceCollection services, Assembly assembly)
    {
        var provider = services.BuildServiceProvider();
        var env = provider.GetRequiredService<IWebHostEnvironment>();
        var config = provider.GetRequiredService<IConfiguration>();
        
        services.AddMarketplaceDatabase(env, config);
        services.AddInfrastructureServices(env, config);
        services.AddUseCases();

        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        services.AddControllers()
            .AddApplicationPart(assembly);
        
        services.AddFluentRequestValidation();
        services.AddAutoMapping();
        services.AddEndpointsApiExplorer();
        services.AddAutoDocumentation<ApiGroups>(env);
    }
}