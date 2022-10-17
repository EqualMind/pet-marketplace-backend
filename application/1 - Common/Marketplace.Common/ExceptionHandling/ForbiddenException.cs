using Microsoft.AspNetCore.Http;

namespace Marketplace.Common.ExceptionHandling;

/// <summary>
/// Исключение "Доступ запрещен"
/// </summary>
public class ForbiddenException : HttpException<ForbiddenExceptionBody>
{
    public ForbiddenException() : base(new ForbiddenExceptionBody())
    {
        
    }

    public ForbiddenException(string message) : base(new ForbiddenExceptionBody { Message = message })
    {
        
    }
}

/// <summary>
/// Тело исключения "Доступ запрещен"
/// </summary>
public class ForbiddenExceptionBody : HttpExceptionBody
{
    public override int HttpStatus => StatusCodes.Status403Forbidden;
    public override string Tag => ExceptionTags.Forbidden;
    public override string Message { get; internal init; } = "Доступ запрещен!";
}