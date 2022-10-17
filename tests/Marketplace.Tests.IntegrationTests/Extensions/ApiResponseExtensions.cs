using Refit;

namespace Marketplace.Tests.IntegrationTests.Extensions;

public static class ApiResponseExtensions
{
    /// <summary>
    /// Извлекает содержимое cookie по ключу
    /// </summary>
    public static string? GetCookie(this IApiResponse response, string name) =>
        response.Headers
            .Where(header => header.Key == "Set-Cookie")
            .SelectMany(header => header.Value)
            .Where(cookie => cookie.StartsWith(name))
            .Select(cookie => cookie.Split("=").ElementAt(1).Split(";").First())
            .FirstOrDefault();
}