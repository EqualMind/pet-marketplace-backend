using Marketplace.Common.ExceptionHandling;

namespace Marketplace.Common.Guards;

public static partial class Guard
{
    /// <summary>
    /// Правила валидации ссылочных типов
    /// </summary>
    public static class Object
    {
        /// <summary>
        /// Проверка что объект существует
        /// </summary>
        /// <param name="value">Проверяемое значение</param>
        /// <param name="message">Сообщение об ошибке</param>
        /// <exception cref="GuardValidationException">Исключение вида базовой валидации типов</exception>
        public static void NotNull(object? value, string message)
        {
            if (value == null) 
                throw new GuardValidationException(message);
        }
        
        /// <summary>
        /// Осуществляет проверку на соответствие полученного объекта указанному условию.
        /// Если проверка не удалась, выбрасывает исключение <see cref="GuardValidationException"/>
        /// </summary>
        /// <param name="value">Проверяемый объект</param>
        /// <param name="successCondition">Условие проверки</param>
        /// <param name="message">Сообщение об ошибке</param>
        /// <typeparam name="T">Тип валидируемого объекта</typeparam>
        /// <exception cref="GuardValidationException">Исключение вида базовой валидации типов</exception>
        public static void Is<T>(T value, Predicate<T> successCondition, string message)
        {
            if (!successCondition(value))
                throw new GuardValidationException(message);
        }
    }
}