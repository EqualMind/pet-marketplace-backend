using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace Marketplace.Common.ExceptionHandling;

/// <summary>
/// Промежуточное ПО для обработки ошибок и вывода корректного сообщения в теле ответа на запрос
/// </summary>
public sealed class HttpExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;

    public HttpExceptionHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);

            if (exception is HttpException httpException)
            {
                await HandleHttpException(context, httpException);
            }
            else
            {
                var exceptionWrapper = new InternalServerErrorException(exception);
                await HandleHttpException(context, exceptionWrapper);
            }
        }
    }
    
    private async Task HandleHttpException(HttpContext context, HttpException exception)
    {
        context.Response.StatusCode = exception.StatusCode;
        context.Response.ContentType = MediaTypeNames.Application.Json;

        await context.Response.WriteAsync(exception.SerializeHttpMessage());
    }
}