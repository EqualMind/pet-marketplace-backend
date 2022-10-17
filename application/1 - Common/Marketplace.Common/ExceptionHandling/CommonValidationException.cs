using Microsoft.AspNetCore.Http;

namespace Marketplace.Common.ExceptionHandling;

/// <summary>
/// Исключение при некорректном запросе
/// </summary>
public class CommonValidationException : HttpException<CommonValidationExceptionBody>
{
    /// <summary>
    /// Создаёт новое исключение при некорректном запросе
    /// </summary>
    /// <param name="validationErrors">Список валидационных сообщений</param>
    public CommonValidationException(List<string> validationErrors) : base(new CommonValidationExceptionBody { ValidationErrors = validationErrors })
    {
        
    }

    /// <summary>
    /// Создаёт новое исключение при некорректном запросе
    /// </summary>
    /// <param name="status">HTTP статус</param>
    /// <param name="message">Сообщение об исключении</param>
    /// <param name="validationErrors">Список валидационных сообщений</param>
    public CommonValidationException(int status, string message, List<string> validationErrors) : base(new CommonValidationExceptionBody(status, message) { ValidationErrors = validationErrors})
    {
        
    }
}

/// <summary>
/// Тело сообщения о некорректном запросе
/// </summary>
public class CommonValidationExceptionBody : HttpExceptionBody
{
    public CommonValidationExceptionBody()
    {
        
    }

    public CommonValidationExceptionBody(int status, string message)
    {
        HttpStatus = status;

        if (!string.IsNullOrWhiteSpace(message))
            Message = message;
    }
    
    public override int HttpStatus { get; } = StatusCodes.Status400BadRequest;
    public override string Tag => ExceptionTags.InvalidRequest;
    public sealed override string Message { get; internal init; } = "Ошибка валидации запроса";

    public List<string> ValidationErrors { get; internal init; } = new();
}