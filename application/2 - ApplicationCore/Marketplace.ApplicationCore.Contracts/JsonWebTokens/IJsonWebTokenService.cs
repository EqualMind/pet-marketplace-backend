namespace Marketplace.ApplicationCore.Contracts.JsonWebTokens;

/// <summary>
/// Сервис для работы с Json Web Token
/// </summary>
public interface IJsonWebTokenService
{
    /// <summary>
    /// Генерирует новый токен доступа в приложение и токен обновления доступа к нему
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    (string AccessToken, string RefreshToken) GenerateAccessTokens(Guid accountId);
}