namespace Marketplace.Common.Extensions;

public static class BooleanExtensions
{
    /// <summary>
    /// Выбрасывает исключение, если значение равно false
    /// </summary>
    /// <param name="bool">Значение</param>
    /// <param name="exception">Исключение</param>
    public static void ThrowIfFalse(this bool @bool, Exception exception)
    {
        if (!@bool) throw exception;
    }
    
    /// <summary>
    /// Выбрасывает исключение, если значение равно true
    /// </summary>
    /// <param name="bool">Значение</param>
    /// <param name="exception">Исключение</param>
    public static void ThrowIfTrue(this bool @bool, Exception exception)
    {
        if (@bool) throw exception;
    }
}