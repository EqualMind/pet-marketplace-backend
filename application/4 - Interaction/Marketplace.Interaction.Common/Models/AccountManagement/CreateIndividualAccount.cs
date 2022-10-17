using AutoMapper;
using FluentValidation;
using Marketplace.ApplicationCore.Domain.Accounts.Contracts;
using Marketplace.ApplicationCore.Operations.AccountManagement;
using Swashbuckle.AspNetCore.Annotations;

namespace Marketplace.Interaction.Common.Models.AccountManagement;

/// <summary>
/// Создание индивидуального аккаунта
/// </summary>
public static class CreateIndividualAccount
{
    /// <summary>
    /// Тело запроса на создание индивидуального аккаунта
    /// </summary>
    public class Request
    {
        [SwaggerSchema("Адрес электронной почты")]
        public string Email { get; set; }
        
        [SwaggerSchema("Пароль")]
        public string Password { get; set; }
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
            CreateMap<Request, CreateAccount.Arguments>();
        }
    }
}