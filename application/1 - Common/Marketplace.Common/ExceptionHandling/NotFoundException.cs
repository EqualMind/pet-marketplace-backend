using Microsoft.AspNetCore.Http;

namespace Marketplace.Common.ExceptionHandling;

/// <summary>
/// Исключение "не удалось найти объект"
/// </summary>
public class NotFoundException : HttpException<NotFoundExceptionBody>
{
    /// <summary>
    /// Создаёт новое исключение "не удалось найти объект"
    /// </summary>
    /// <param name="type">Тип объекта</param>
    /// <param name="args">Дополнительные аргументы</param>
    public NotFoundException(Type type, object? args) : base(new NotFoundExceptionBody(type, args))
    {
        
    }
}

/// <summary>
/// Тело исключения "не удалось найти объект"
/// </summary>
public class NotFoundExceptionBody : HttpExceptionBody
{
    public NotFoundExceptionBody()
    {
        
    }
    
    public NotFoundExceptionBody(Type type, object? args)
    {
        Message = $"{type.Name} was not found!";
        Args = args;
    }
    
    public override int HttpStatus => StatusCodes.Status404NotFound;
    public override string Tag => ExceptionTags.NotFound;
    public sealed override string Message { get; internal init; }
    public object? Args { get; }
}