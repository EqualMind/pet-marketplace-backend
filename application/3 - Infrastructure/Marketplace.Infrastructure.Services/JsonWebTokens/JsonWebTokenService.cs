using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Marketplace.ApplicationCore.Contracts.JsonWebTokens;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Marketplace.Infrastructure.Services.JsonWebTokens;

public class JsonWebTokenService : IJsonWebTokenService
{
    private readonly JsonWebTokenSettings settings;
    private readonly IJsonWebTokenSigningKeyProvider signingKeyProvider;
    private readonly IJsonWebTokenEncryptingKeyProvider encryptingKeyProvider;

    public JsonWebTokenService(IOptions<JsonWebTokenSettings> options, IJsonWebTokenSigningKeyProvider signingKeyProvider, IJsonWebTokenEncryptingKeyProvider encryptingKeyProvider)
    {
        settings = options.Value;
        this.signingKeyProvider = signingKeyProvider;
        this.encryptingKeyProvider = encryptingKeyProvider;
    }

    public (string AccessToken, string RefreshToken) GenerateAccessTokens(Guid accountId)
    {
        var refreshToken = Guid.NewGuid();
        
        var claims = new Claim[]
        {
            new(ClaimTypes.PrimarySid, accountId.ToString()),
            new(ClaimTypes.Sid, refreshToken.ToString())
        };
        
        var signingCredentials = new SigningCredentials(
            signingKeyProvider.SecurityKey, 
            signingKeyProvider.SigningAlgorithm);
        
        var encryptingCredentials = new EncryptingCredentials(
            encryptingKeyProvider.SecurityKey, 
            encryptingKeyProvider.SigningAlgorithm, 
            encryptingKeyProvider.EncryptingAlgorithm);

        var tokenHandler = new JwtSecurityTokenHandler();
        var now = DateTime.Now;

        var token = tokenHandler.CreateJwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            subject: new ClaimsIdentity(claims),
            notBefore: now,
            expires: now.AddMinutes(settings.LifetimeMinutes),
            issuedAt: now,
            signingCredentials: signingCredentials,
            encryptingCredentials: encryptingCredentials);

        return (tokenHandler.WriteToken(token), refreshToken.ToString());
    }
}