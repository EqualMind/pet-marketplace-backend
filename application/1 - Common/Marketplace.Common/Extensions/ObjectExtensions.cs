namespace Marketplace.Common.Extensions;

public static class ObjectExtensions
{
    /// <summary>
    /// Выбрасывает исключение, если объект не инициализирован
    /// </summary>
    public static void ThrowIfNull<T>(this T value, Exception exception) where T : class?
    {
        if (value == null) 
            throw exception;
    }
}