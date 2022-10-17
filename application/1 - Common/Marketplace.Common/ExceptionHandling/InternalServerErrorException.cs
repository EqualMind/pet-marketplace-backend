using Microsoft.AspNetCore.Http;

namespace Marketplace.Common.ExceptionHandling;

/// <summary>
/// Неизвестное исключение
/// </summary>
public class InternalServerErrorException : HttpException<InternalServerErrorExceptionBody>
{
    /// <summary>
    /// Создаёт исключение вида InternalServerError
    /// </summary>
    public InternalServerErrorException() : base(new InternalServerErrorExceptionBody())
    {
        
    }

    /// <summary>
    /// Создаёт исключение вида InternalServerError с указанным сообщением
    /// </summary>
    /// <param name="message">Сообщение об исключении</param>
    public InternalServerErrorException(string message) : base(new InternalServerErrorExceptionBody { Message = message })
    {
        
    }

    /// <summary>
    /// Оборачивает исключение в исключение вида InternalServerError
    /// </summary>
    /// <param name="innerException">Оборачиваемое исключение</param>
    public InternalServerErrorException(Exception innerException) : base(new InternalServerErrorExceptionBody{ Message = innerException.Message }, innerException)
    {
        
    }
}

/// <summary>
/// Тело неизвестной ошибки (Всегда 500 статус)
/// </summary>
public class InternalServerErrorExceptionBody : HttpExceptionBody
{
    public InternalServerErrorExceptionBody()
    {
        
    }
    
    public override int HttpStatus => StatusCodes.Status500InternalServerError;

    public override string Tag { get; } = ExceptionTags.InternalServerError;

    public override string Message { get; internal init; } = "Something went wrong!";
}