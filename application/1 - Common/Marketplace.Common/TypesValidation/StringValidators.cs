namespace Marketplace.Common.TypesValidation;

/// <summary>
/// Правила валидации для строк
/// </summary>
/// <remarks>
/// Используются вместе с FluentValidation
/// </remarks>
public static class StringValidators
{
    /// <summary>
    /// Проверяет, является ли значение в строке валидным GUID
    /// </summary>
    /// <param name="value">Проверяемое значение</param>
    public static bool IsGuid(string? value) => value != null && Guid.TryParse(value, out var result);
}