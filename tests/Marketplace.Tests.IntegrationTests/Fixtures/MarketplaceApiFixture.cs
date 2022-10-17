using Marketplace.Common;
using Marketplace.Common.Architecture;
using Marketplace.Infrastructure.Database;
using Marketplace.Interaction.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Tests.IntegrationTests.Fixtures;

/// <summary>
/// Фикстура с настроенным окружением для интеграционного тестирования публичного API торговой площадки
/// </summary>
public sealed class MarketplaceApiFixture : IDisposable
{
    private readonly WebApplicationFactory<Program> factory;

    public MarketplaceApiFixture()
    {
        factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment(EnvironmentList.AutoTesting);
                builder.ConfigureServices(ConfigureTestServices);
            });

        Client = factory.CreateClient();
        Storage = factory.Services.GetRequiredService<IApplicationStorage>();
        
        var dbContext = factory.Services.GetRequiredService<MarketplaceContext>();
        dbContext.Database.Migrate();
        
        var config = factory.Services.GetRequiredService<IConfiguration>();
        var env = factory.Services.GetRequiredService<IWebHostEnvironment>();

        var seeder = new Seeder(config, env, factory.Services);

        var seedingTask = Task.Run(async () => await ConfigureSeeder(seeder));
        seedingTask.Wait();
    }
    
    /// <summary>
    /// <see cref="HttpClient"/> Rest API сервера торговой площадки
    /// </summary>
    public readonly HttpClient Client;
    
    /// <summary>
    /// Доступ к хранилищу данных торговой площадки
    /// </summary>
    public readonly IApplicationStorage Storage;

    /// <inheritdoc cref="ServiceProviderServiceExtensions.GetRequiredService{T}"/>
    public T GetRequiredService<T>() where T : class => factory.Services.GetRequiredService<T>();
    
    /// <summary>
    /// Настройка коллекции сервисов для автотестирования
    /// </summary>
    private void ConfigureTestServices(IServiceCollection services)
    {
        
    }

    /// <summary>
    /// Настройки предзаполнения тестовой базы данными
    /// </summary>
    private static async Task ConfigureSeeder(Seeder seeder)
    {
        
    }

    public void Dispose()
    {
        factory.Dispose();
    }
}