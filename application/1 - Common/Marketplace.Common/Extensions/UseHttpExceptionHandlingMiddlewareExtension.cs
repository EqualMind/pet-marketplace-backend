using Marketplace.Common.ExceptionHandling;
using Microsoft.AspNetCore.Builder;

namespace Marketplace.Common.Extensions;

public static class UseHttpExceptionHandlingMiddlewareExtension
{
    /// <summary>
    /// Регистрирует промежуточное ПО для обработки <see cref="HttpException"/>
    /// </summary>
    /// <param name="app"></param>
    public static void UseHttpExceptionHandlingMiddleware(this WebApplication app)
    {
        app.UseMiddleware<HttpExceptionHandlingMiddleware>();
    }
}