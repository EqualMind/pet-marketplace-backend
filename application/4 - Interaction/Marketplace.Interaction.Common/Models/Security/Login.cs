using AutoMapper;
using FluentValidation;
using Marketplace.ApplicationCore.Operations.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Marketplace.Interaction.Common.Models.Security;

/// <summary>
/// Модели для запроса авторизации в системе
/// </summary>
public static class Login
{
    /// <summary>
    /// Тело запроса на авторизацию в системе
    /// </summary>
    public class Request
    {
        [SwaggerSchema("Адрес электронной почты")]
        public string Email { get; init; }

        [SwaggerSchema("Пароль")]
        public string Password { get; init; }
    }

    internal class RequestValidation : AbstractValidator<Request>
    {
        public RequestValidation()
        {
            RuleFor(request => request.Email).NotEmpty().EmailAddress();
            RuleFor(request => request.Password).NotEmpty();
        }
    }
    
    internal class RequestMappingProfile : Profile
    {
        public RequestMappingProfile()
        {
            CreateMap<Request, Authorization.Arguments>();
        }
    }
}