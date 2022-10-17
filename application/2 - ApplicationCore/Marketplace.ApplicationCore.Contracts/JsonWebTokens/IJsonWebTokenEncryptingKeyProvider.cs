using Microsoft.IdentityModel.Tokens;

namespace Marketplace.ApplicationCore.Contracts.JsonWebTokens;

/// <summary>
/// Поставщик ключа шифрования Json Web Token (приватный)
/// </summary>
public interface IJsonWebTokenEncryptingKeyProvider
{
    /// <summary>
    /// Алгоритм для подписи
    /// </summary>
    string SigningAlgorithm { get; }
    
    /// <summary>
    /// Алгоритм для шифрования
    /// </summary>
    string EncryptingAlgorithm { get; }
    
    /// <summary>
    /// Сформированный ключ шифрования
    /// </summary>
    SecurityKey SecurityKey { get; }
}