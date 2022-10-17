namespace Marketplace.ApplicationCore.Contracts.JsonWebTokens;

/// <summary>
/// Константные значения, используемые в работе с Json Web Token
/// </summary>
public static class JsonWebTokenConstants
{
    public static class Cookies
    {
        /// <summary>
        /// Токен доступа
        /// </summary>
        public const string AccessToken = "x-access-token";
        
        /// <summary>
        /// Токен обновления доступа
        /// </summary>
        public const string RefreshToken = "x-refresh-token";
    }
}