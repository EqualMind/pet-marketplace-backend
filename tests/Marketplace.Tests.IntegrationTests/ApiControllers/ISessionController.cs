using Marketplace.Interaction.Common.Models.Security;
using Refit;

namespace Marketplace.Tests.IntegrationTests.ApiControllers;

/// <summary>
/// Набор запросов к контроллеру управления сессиями
/// </summary>
public interface ISessionController
{
    /// <summary>
    /// Авторизация в системе
    /// </summary>
    /// <param name="request">Тело запроса</param>
    [Post("/api/session/login")]
    Task<IApiResponse> Login([Body] Login.Request request);

    /// <summary>
    /// Обновление токена доступа
    /// </summary>
    [Get("/api/session/refresh")]
    Task<IApiResponse> Refresh();

    /// <summary>
    /// Выход из сессии
    /// </summary>
    [Post("/api/session/logout")]
    Task<IApiResponse> Logout();
}