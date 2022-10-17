using System.Text;
using Marketplace.ApplicationCore.Contracts.JsonWebTokens;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Marketplace.Infrastructure.Services.JsonWebTokens;

/// <summary>
/// Симметричный ключ шифрования JWT
/// </summary>
public class JsonWebTokenSigningSymmetricKeyProviderProvider : IJsonWebTokenSigningKeyProvider, IJsonWebTokenSigningDecodingKeyProvider
{
    /// <summary>
    /// Создаёт новый симметричный ключ шифрования JWT
    /// </summary>
    /// <param name="options">Настройки генерации токенов</param>
    public JsonWebTokenSigningSymmetricKeyProviderProvider(IOptions<JsonWebTokenSettings> options)
    {
        SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SigningSecretKey));
    }

    public string SigningAlgorithm => SecurityAlgorithms.HmacSha256;
    public SecurityKey SecurityKey { get; }
}