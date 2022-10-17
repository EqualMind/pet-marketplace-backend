using Microsoft.AspNetCore.Http;

namespace Marketplace.Common.ExceptionHandling;

/// <summary>
/// Исключение при базовой валидации типов
/// </summary>
public class GuardValidationException : HttpException<GuardValidationExceptionBody>
{
    /// <summary>
    /// Создаёт новое исключение при базовой валидации типов
    /// </summary>
    /// <param name="message">Сообщение об исключении</param>
    public GuardValidationException(string message) : base(new GuardValidationExceptionBody{ Message = message })
    {
        
    }
}

/// <summary>
/// Тело исключения при базовой валидации типов
/// </summary>
public class GuardValidationExceptionBody : HttpExceptionBody
{
    public override int HttpStatus => StatusCodes.Status400BadRequest;

    public override string Tag => ExceptionTags.GuardValidationFailed;

    public override string Message { get; internal init; } = "The value is invalid!";
}