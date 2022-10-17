using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Marketplace.Common.ExceptionHandling;

/// <summary>
/// Базовый класс для кастомных сообщений об исключении, интерпретируемых промежуточным ПО
/// </summary>
public abstract class HttpException : Exception
{
    protected HttpException(string message) : base(message)
    {
        
    }

    protected HttpException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
    
    /// <summary>
    /// Возвращает HTTP статус ответа сервера на запрос
    /// </summary>
    public abstract int StatusCode { get; }

    /// <summary>
    /// Осуществляет сериализацию исключения в текстовое представление для вывода в ответе на запрос
    /// </summary>
    public abstract string SerializeHttpMessage();
}

/// <inheritdoc cref="HttpException"/>
/// <typeparam name="TBody">Тип тела исключения</typeparam>
public abstract class HttpException<TBody> : HttpException where TBody : HttpExceptionBody, new()
{
    protected HttpException(Exception innerException) : base(innerException.Message, innerException)
    {
        Body = new TBody
        {
            Message = innerException.Message
        };
    }
    
    protected HttpException(TBody body) : base(body.Message)
    {
        Body = body;
    }

    protected HttpException(TBody body, Exception innerException) : base(body.Message, innerException)
    {
        Body = body;
    }
    
    /// <summary>
    /// Тело сообщения об исключении
    /// </summary>
    public TBody Body { get; }

    public override int StatusCode => Body.HttpStatus;

    public override string SerializeHttpMessage() => JsonSerializer.Serialize(Body);
}

/// <summary>
/// Базовый класс для тела Http исключения
/// </summary>
/// <remarks>
/// Данное тело будет выводить информацию в виде JSON в теле ответа
/// </remarks>
public abstract class HttpExceptionBody
{
    /// <summary>
    /// <see cref="StatusCodes">HTTP Статус ответа</see> для данного исключения
    /// </summary>
    public abstract int HttpStatus { get; }

    /// <summary>
    /// Тег сообщения об исключении
    /// </summary>
    public abstract string Tag { get; }
    
    /// <summary>
    /// Информация об исключении
    /// </summary>
    public abstract string Message { get; internal init; }
}