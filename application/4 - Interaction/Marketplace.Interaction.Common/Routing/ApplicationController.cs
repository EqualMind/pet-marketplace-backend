using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using Marketplace.ApplicationCore.Contracts.JsonWebTokens;
using Marketplace.Common.Architecture;
using Marketplace.Common.ExceptionHandling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Interaction.Common.Routing;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public abstract class ApplicationController : ExtendedController
{
    /// <inheritdoc cref="IMapper"/>
    protected IMapper Mapper => HttpContext.RequestServices.GetRequiredService<IMapper>();
    
    /// <summary>
    /// Осуществляет валидацию модели, если в системе зарегистрирован валидатор
    /// </summary>
    /// <param name="value">Валидируемая модель</param>
    /// <typeparam name="T">Тип модели</typeparam>
    /// <exception cref="CommonValidationException">Ошибка валидации модели</exception>
    protected async Task ValidateAsync<T>(T value)
    {
        var validator = HttpContext.RequestServices.GetService<IValidator<T>>();
        if (validator == null) return;

        var validationResult = await validator.ValidateAsync(value);

        if (!validationResult.IsValid)
        {
            throw new CommonValidationException(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }
    }

    #region Claims
    
    /// <summary>
    /// Возвращает утверждение из токена доступа, если найдено
    /// </summary>
    /// <param name="type">Тип утверждения</param>
    protected string? GetClaim(string type) => HttpContext.User.Claims.FirstOrDefault(x => x.Type == type)?.Value;

    /// <summary>
    /// Извлекает из токена доступа идентификатор текущего пользователя
    /// </summary>
    protected string? UserId => GetClaim(ClaimTypes.PrimarySid);
    
    /// <summary>
    /// Извлекает из токена доступа сгенерированный токен обновления сессии
    /// </summary>
    protected string? RefreshToken => GetClaim(ClaimTypes.Sid);
    
    /// <summary>
    /// Вычисленный хэш токена обновления сессии
    /// </summary>
    protected string? RefreshTokenHash => Request.Cookies[JsonWebTokenConstants.Cookies.RefreshToken];
    
    #endregion
}