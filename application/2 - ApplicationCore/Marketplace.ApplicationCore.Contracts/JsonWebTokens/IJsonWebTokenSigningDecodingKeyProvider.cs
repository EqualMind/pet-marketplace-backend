using Microsoft.IdentityModel.Tokens;

namespace Marketplace.ApplicationCore.Contracts.JsonWebTokens;

/// <summary>
/// Поставщик ключа для проверки подписи (публичный)
/// </summary>
public interface IJsonWebTokenSigningDecodingKeyProvider
{
    /// <summary>
    /// Сформированный ключ для проверки подписи
    /// </summary>
    SecurityKey SecurityKey { get; }
}