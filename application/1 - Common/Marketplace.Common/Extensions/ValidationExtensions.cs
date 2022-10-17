using FluentValidation;
using Marketplace.Common.ExceptionHandling;
using Microsoft.AspNetCore.Http;

namespace Marketplace.Common.Extensions;

public static class ValidationExtensions
{
    /// <summary>
    /// Осуществляет валидацию объекта
    /// </summary>
    /// <param name="validator">Валидатор</param>
    /// <param name="value">Валидируемый объект</param>
    /// <param name="message">Сообщение об исключении</param>
    /// <typeparam name="T">Тип объекта</typeparam>
    /// <exception cref="CommonValidationException">Валидация завершилась с ошибкой</exception>
    public static async Task ValidateObjectAsync<T>(this IValidator<T> validator, T value, string message)
    {
        var result = await validator.ValidateAsync(value);

        if (!result.IsValid)
        {
            throw new CommonValidationException(StatusCodes.Status400BadRequest, message, result.Errors.Select(e => e.ErrorMessage).ToList());
        }
    }
}