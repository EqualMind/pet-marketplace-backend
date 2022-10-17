using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Common.Architecture;

/// <summary>
/// Механизм предзаполнения базы данных через коллекцию репозиториев
/// </summary>
public sealed class Seeder
{
    private readonly IConfiguration config;
    private readonly IWebHostEnvironment env;
    private readonly IServiceProvider provider;
    
    public Seeder(IConfiguration config, IWebHostEnvironment env, IServiceProvider provider)
    {
        this.config = config;
        this.env = env;
        this.provider = provider;
    }

    /// <summary>
    /// Применяет спецификацию предзаполнения базы данных в изолированном <see cref="IServiceScope"/>
    /// </summary>
    /// <typeparam name="TSpecification">Тип спецификации с пустым конструктором</typeparam>
    public async Task ApplyAsync<TSpecification>() where TSpecification : SeedSpecification
    {
        using var scope = provider.CreateScope();
        
        var specification = ActivatorUtilities.CreateInstance<TSpecification>(scope.ServiceProvider);
        var storage = scope.ServiceProvider.GetRequiredService<IApplicationStorage>();
        
        if (!await specification.CanBeAppliedAsync(storage, config, env)) return;
        await specification.ApplyAsync(storage, config, env);
    }
}