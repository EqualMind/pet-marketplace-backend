using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Marketplace.ApplicationCore.Contracts.JsonWebTokens;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Marketplace.Infrastructure.Services.JsonWebTokens;

/// <summary>
/// Поставщик симметричного ключа шифрования Json Web Token
/// </summary>
public class JsonWebTokenEncryptingSymmetricKeyProvider : IJsonWebTokenEncryptingKeyProvider, IJsonWebTokenEncryptingDecodingKeyProvider
{
    /// <summary>
    /// Создаёт новый объект поставщика симметричного ключа шифрования Json Web Token
    /// </summary>
    public JsonWebTokenEncryptingSymmetricKeyProvider(IOptions<JsonWebTokenSettings> options)
    {
        SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.EncryptingSecretKey));
    }

    public string SigningAlgorithm => JwtConstants.DirectKeyUseAlg;
    public string EncryptingAlgorithm => SecurityAlgorithms.Aes256CbcHmacSha512;
    public SecurityKey SecurityKey { get; }
}