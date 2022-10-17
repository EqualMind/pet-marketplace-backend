namespace Marketplace.ApplicationCore.Contracts.Encrypting;

/// <summary>
/// Интерфейс для сервиса хэширования и валидации строк
/// </summary>
public interface IStringEncoder
{
    /// <summary>
    /// Осуществляет проверку на совпадение строки с хэшем
    /// </summary>
    /// <param name="value">Строка для сравнения</param>
    /// <param name="hash">Хэш</param>
    public bool Compare(string value, string hash);

    /// <summary>
    /// Осуществляет вычисление хэша строки
    /// </summary>
    /// <param name="value">Строка с паролем для хэширования</param>
    public string Hash(string value);
}