using System.Net.Mime;
using Marketplace.ApplicationCore.Operations.AccountManagement;
using Marketplace.Common.ExceptionHandling;
using Marketplace.Interaction.Common;
using Marketplace.Interaction.Common.Models.AccountManagement;
using Marketplace.Interaction.Common.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Marketplace.Interaction.Api.Controllers;

[ApiExplorerSettings(GroupName = ApiGroups.AccountManagement)]
[SwaggerTag("Контроллер по управлению данными аккаунта")]
public class AccountsController : ApplicationController
{
    [AllowAnonymous]
    [HttpPost("create")]
    [SwaggerOperation("Создаёт новый индивидуальный аккаунт")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Аккаунт успешно создан в системе")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Некорректный запрос", typeof(CommonValidationExceptionBody), MediaTypeNames.Application.Json)]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Некорректный запрос", typeof(BadRequestExceptionBody), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Create([FromBody] CreateIndividualAccount.Request request)
    {
        await ValidateAsync(request);
        await InitOperation<CreateAccount.Operation>().ExecuteAsync(Mapper.Map<CreateAccount.Arguments>(request));
        
        return NoContent();
    }
}