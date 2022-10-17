using Marketplace.ApplicationCore.Contracts.Encrypting;
using Marketplace.ApplicationCore.Contracts.JsonWebTokens;
using Marketplace.Infrastructure.Services.Encrypting;
using Marketplace.Infrastructure.Services.JsonWebTokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Marketplace.Infrastructure.Services;

public static class AddInfrastructureServicesExtension
{
    public static void AddInfrastructureServices(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
    {
        services.AddJsonWebTokens(env, config);
        services.AddSingleton<IStringEncoder, StringEncoder>();
    }
    
    /// <summary>
    /// Регистрация и настройка сервисов работы с Json Web Tokens
    /// </summary>
    private static void AddJsonWebTokens(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
    {
        if (!env.IsProduction())
        {
            IdentityModelEventSource.ShowPII = true;
        }
        
        services.Configure<JsonWebTokenSettings>(config.GetSection(nameof(JsonWebTokenSettings)));

        services.AddSingleton<JsonWebTokenSigningSymmetricKeyProviderProvider>();
        services.AddSingleton<JsonWebTokenEncryptingSymmetricKeyProvider>();
        
        services.AddSingleton<IJsonWebTokenSigningKeyProvider>(provider => provider.GetRequiredService<JsonWebTokenSigningSymmetricKeyProviderProvider>());
        services.AddSingleton<IJsonWebTokenSigningDecodingKeyProvider>(provider => provider.GetRequiredService<JsonWebTokenSigningSymmetricKeyProviderProvider>());
        
        services.AddSingleton<IJsonWebTokenEncryptingKeyProvider, JsonWebTokenEncryptingSymmetricKeyProvider>(provider => provider.GetRequiredService<JsonWebTokenEncryptingSymmetricKeyProvider>());
        services.AddSingleton<IJsonWebTokenEncryptingDecodingKeyProvider, JsonWebTokenEncryptingSymmetricKeyProvider>(provider => provider.GetRequiredService<JsonWebTokenEncryptingSymmetricKeyProvider>());
        
        services.AddSingleton<IJsonWebTokenService, JsonWebTokenService>();
        
        var provider = services.BuildServiceProvider();
        var settings = provider.GetRequiredService<IOptions<JsonWebTokenSettings>>().Value;
        
        var symmetricDecodingKeyProvider = provider.GetRequiredService<IJsonWebTokenSigningDecodingKeyProvider>();
        var encryptingDecodingKeyProvider = provider.GetRequiredService<IJsonWebTokenEncryptingDecodingKeyProvider>();
        
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[JsonWebTokenConstants.Cookies.AccessToken];
                        return Task.CompletedTask;
                    }
                };
                
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = symmetricDecodingKeyProvider.SecurityKey,
                    TokenDecryptionKey = encryptingDecodingKeyProvider.SecurityKey,
                    
                    ValidateIssuer = true,
                    ValidIssuer = settings.Issuer,
                    
                    ValidateAudience = true,
                    ValidAudience = settings.Audience,
                    
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(5)
                };
            });
    }
}