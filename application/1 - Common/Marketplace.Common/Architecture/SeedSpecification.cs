using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Marketplace.Common.Architecture;

/// <summary>
/// Спецификация по предзаполнению базы данных
/// </summary>
public abstract class SeedSpecification
{
    /// <summary>
    /// Осуществляет проверку перед применением спецификации по предзаполнению базы данных
    /// </summary>
    /// <param name="storage">Хранилище данных приложения</param>
    /// <param name="config">Конфигурация приложения</param>
    /// <param name="env">Среда выполнения приложения</param>
    public abstract Task<bool> CanBeAppliedAsync(IApplicationStorageReader storage, IConfiguration config, IWebHostEnvironment env);

    /// <summary>
    /// Применяет спецификацию по предзаполнению базы данных
    /// </summary>
    /// <param name="storage">Хранилище данных приложения</param>
    /// <param name="config">Конфигурация приложения</param>
    /// <param name="env">Среда выполнения приложения</param>
    public abstract Task ApplyAsync(IApplicationStorage storage, IConfiguration config, IWebHostEnvironment env);
}