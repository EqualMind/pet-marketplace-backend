using Marketplace.Common.Architecture;
using Marketplace.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Marketplace.Tests.IntegrationTests.Contracts;

/// <summary>
/// Базовый класс для всех спецификаций предзаполнения данными в автотестах
/// </summary>
public abstract class AutoTestingSeedSpecificationBase : SeedSpecification
{
    public override async Task<bool> CanBeAppliedAsync(IApplicationStorageReader storage, IConfiguration config, IWebHostEnvironment env)
        => await CanBeAppliedAdditionallyAsync(storage, config, env) && env.IsAutoTesting();

    /// <summary>
    /// Дополнительное условие для проверки возможности применения данной спецификации
    /// </summary>
    protected virtual Task<bool> CanBeAppliedAdditionallyAsync(IApplicationStorageReader storage, IConfiguration config, IWebHostEnvironment env)
        => Task.FromResult(true);
}