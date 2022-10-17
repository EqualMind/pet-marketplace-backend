using System.Net.Mime;
using Marketplace.ApplicationCore.Contracts.JsonWebTokens;
using Marketplace.ApplicationCore.Operations.Security;
using Marketplace.Common.ExceptionHandling;
using Marketplace.Interaction.Common;
using Marketplace.Interaction.Common.Models.Security;
using Marketplace.Interaction.Common.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Marketplace.Interaction.Api.Controllers;

[ApiExplorerSettings(GroupName = ApiGroups.Security)]
[SwaggerTag("Контроллер для управления сессиями в системе")]
public class SessionController : ApplicationController
{
    [AllowAnonymous]
    [HttpPost("login")]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation("Авторизация в системе")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Авторизация прошла успешно")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Некорректный запрос", typeof(CommonValidationExceptionBody), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Отказано в доступе", typeof(ForbiddenExceptionBody), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Аккаунт не найден", typeof(NotFoundExceptionBody), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Login([FromBody] Login.Request request)
    {
        await ValidateAsync(request);

        var result = await InitOperation<Authorization.Operation>().ExecuteAsync(Mapper.Map<Authorization.Arguments>(request));
        
        Response.Cookies.Append(JsonWebTokenConstants.Cookies.AccessToken, result.AccessToken, new CookieOptions { HttpOnly = true });
        Response.Cookies.Append(JsonWebTokenConstants.Cookies.RefreshToken, result.RefreshTokenHash, new CookieOptions { HttpOnly = true });

        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("refresh")]
    [SwaggerOperation("Попытка обновления сессии в системе")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Обновление данных сессии прошло успешно")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Данные для обновления сессии повреждены", typeof(ForbiddenExceptionBody), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Аккаунт не найден", typeof(ForbiddenExceptionBody), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Токена обновления сессии не существует", typeof(ForbiddenExceptionBody), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Невалидный токен обновления сессии", typeof(ForbiddenExceptionBody), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Refresh()
    {
        var arguments = new RefreshSession.Arguments(UserId, RefreshToken, RefreshTokenHash);
        var result = await InitOperation<RefreshSession.Operation>().ExecuteAsync(arguments);

        Response.Cookies.Append(JsonWebTokenConstants.Cookies.AccessToken, result.AccessToken, new CookieOptions { HttpOnly = true });
        Response.Cookies.Append(JsonWebTokenConstants.Cookies.RefreshToken, result.RefreshTokenHash, new CookieOptions { HttpOnly = true });

        return NoContent();
    }
    
    [HttpPost("logout")]
    [SwaggerOperation("Выход из системы")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Выход из системы прошел успешно")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Данные для обновления сессии повреждены", typeof(ForbiddenExceptionBody), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Аккаунт не найден", typeof(ForbiddenExceptionBody), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Logout()
    {
        var arguments = new Logout.Arguments(UserId, RefreshToken);
        await InitOperation<Logout.Operation>().ExecuteAsync(arguments);
        
        Response.Cookies.Delete(JsonWebTokenConstants.Cookies.AccessToken);
        Response.Cookies.Delete(JsonWebTokenConstants.Cookies.RefreshToken);

        return NoContent();
    }
}