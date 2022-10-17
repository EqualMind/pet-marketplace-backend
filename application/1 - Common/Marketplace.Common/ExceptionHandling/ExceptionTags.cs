namespace Marketplace.Common.ExceptionHandling;

/// <summary>
/// Список тегов для исключений
/// </summary>
public static class ExceptionTags
{
    /// <summary>
    /// Валидация значения выбросила исключение
    /// </summary>
    public static readonly string GuardValidationFailed = "guard-validation-failed";

    /// <summary>
    /// Неизвестное исключение сервера
    /// </summary>
    public static readonly string InternalServerError = "internal-server-error";

    /// <summary>
    /// Некорректный запрос
    /// </summary>
    public static readonly string InvalidRequest = "invalid-request";

    /// <summary>
    /// Не удалось найти объект
    /// </summary>
    public static readonly string NotFound = "not-found";

    /// <summary>
    /// Доступ запрещен
    /// </summary>
    public static readonly string Forbidden = "forbidden";

    /// <summary>
    /// Некорректный запрос
    /// </summary>
    public static readonly string BadRequest = "bad-request";
}