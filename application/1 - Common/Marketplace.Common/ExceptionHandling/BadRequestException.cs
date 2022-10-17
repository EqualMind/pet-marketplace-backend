using Microsoft.AspNetCore.Http;

namespace Marketplace.Common.ExceptionHandling;

/// <summary>
/// Исключение "Некорректный запрос"
/// </summary>
public class BadRequestException : HttpException<BadRequestExceptionBody>
{
    public BadRequestException() : base(new BadRequestExceptionBody())
    {
        
    }

    public BadRequestException(string message) : base(new BadRequestExceptionBody{ Message = message })
    {
        
    }
}

/// <summary>
/// Тело исключения "Некорректный запрос"
/// </summary>
public class BadRequestExceptionBody : HttpExceptionBody
{
    public override int HttpStatus => StatusCodes.Status400BadRequest;
    public override string Tag => ExceptionTags.BadRequest;
    public override string Message { get; internal init; } = "Некорректный запрос!";
}