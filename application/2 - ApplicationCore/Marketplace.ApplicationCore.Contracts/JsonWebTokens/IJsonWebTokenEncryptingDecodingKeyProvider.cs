using Microsoft.IdentityModel.Tokens;

namespace Marketplace.ApplicationCore.Contracts.JsonWebTokens;

/// <summary>
/// Поставщик ключа шифрования Json Web Token (публичный)
/// </summary>
public interface IJsonWebTokenEncryptingDecodingKeyProvider
{
    /// <summary>
    /// Сформированный ключ шифрования
    /// </summary>
    SecurityKey SecurityKey { get; }
}