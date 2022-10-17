namespace Marketplace.ApplicationCore.Contracts.JsonWebTokens;

/// <summary>
/// Настройки Json Web Tokens
/// </summary>
public class JsonWebTokenSettings
{
    /// <summary>
    /// Издатель токена
    /// </summary>
    public string Issuer { get; set; } = "marketplace.auth.server";
    
    /// <summary>
    /// Получатель токена
    /// </summary>
    public string Audience { get; set; } = "marketplace.spa.client";
    
    /// <summary>
    /// Ключ для подписи токена
    /// </summary>
    public string SigningSecretKey { get; set; } = "z%C*F-JaNdRgUkXp2s5u8x/A?D(G+KbPeShVmYq3t6w9y$B&E)H@McQfTjWnZr4u";
    
    /// <summary>
    /// Ключ для шифрования токена
    /// </summary>
    public string EncryptingSecretKey { get; set; } = "@MbQeThWmZq4t7w!z%C*F-JaNdRfUjXn2r5u8x/A?D(G+KbPeShVkYp3s6v9y$B&";
    
    /// <summary>
    /// Время жизни токена в минутах
    /// </summary>
    public int LifetimeMinutes { get; set; } = 1;
}