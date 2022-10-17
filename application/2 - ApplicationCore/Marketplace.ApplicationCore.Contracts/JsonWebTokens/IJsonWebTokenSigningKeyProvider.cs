using Microsoft.IdentityModel.Tokens;

namespace Marketplace.ApplicationCore.Contracts.JsonWebTokens;

/// <summary>
/// Поставщик ключа для создания подписи (приватный)
/// </summary>
public interface IJsonWebTokenSigningKeyProvider
{
    /// <summary>
    /// Используемый алгоритм подписания
    /// </summary>
    string SigningAlgorithm { get; }
    
    /// <summary>
    /// Сформированный ключ для подписи
    /// </summary>
    SecurityKey SecurityKey { get; }
}