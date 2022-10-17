using System.Text.RegularExpressions;
using Marketplace.Common.ExceptionHandling;

namespace Marketplace.Common.Guards;

/// <summary>
/// Правила базовой валидации типов
/// </summary>
public static partial class Guard
{
    /// <summary>
    /// Правила валидации строк
    /// </summary>
    public static class String
    {
        /// <summary>
        /// Проверка строки на пустое значение
        /// </summary>
        /// <param name="value">Проверяемое значение</param>
        /// <param name="message">Сообщение об ошибке</param>
        /// <exception cref="GuardValidationException">Исключение вида базовой валидации типов</exception>
        public static void IsNotNullOrWhitespace(string value, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new GuardValidationException(message);
        }
        
        /// <summary>
        /// Проверка адреса электронной почты на валидность
        /// </summary>
        /// <param name="value">Проверяемое значение</param>
        /// <param name="message">Сообщение об ошибке</param>
        /// <exception cref="GuardValidationException">Исключение вида базовой валидации типов</exception>
        public static void IsEmail(string value, string message)
        {
            if (!Regex.IsMatch(value, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                throw new GuardValidationException(message);
        }
    }
}