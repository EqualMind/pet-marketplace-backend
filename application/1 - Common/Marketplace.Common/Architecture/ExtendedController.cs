using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Common.Architecture;

/// <summary>
/// Расширенный <see cref="ControllerBase"/> базовый класс контроллеров
/// </summary>
public abstract class ExtendedController : ControllerBase
{
    /// <summary>
    /// Осуществляет инициализацию указанной операции из контейнера зависимостей
    /// </summary>
    /// <typeparam name="TOperation">Тип операции</typeparam>
    protected TOperation InitOperation<TOperation>() where TOperation : IOperation
        => HttpContext.RequestServices.GetRequiredService<TOperation>();
}