using System.Net;
using Marketplace.ApplicationCore.Contracts.Encrypting;
using Marketplace.ApplicationCore.Contracts.JsonWebTokens;
using Marketplace.ApplicationCore.Domain.Accounts.Contracts;
using Marketplace.ApplicationCore.Domain.Accounts.Entities;
using Marketplace.Infrastructure.Database.Contracts;
using Marketplace.Interaction.Common.Models.Security;
using Marketplace.Tests.IntegrationTests.Contracts;
using Marketplace.Tests.IntegrationTests.Extensions;
using Marketplace.Tests.IntegrationTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Marketplace.Tests.IntegrationTests.Testing.Security;

/// <summary>
/// Тесты связанные с управлением сессиями в системе
/// </summary>
public class SessionTests : MarketplaceApiTestsBase
{
    public SessionTests(MarketplaceApiFixture fixture) : base(fixture)
    {
        
    }

    [Fact(DisplayName = "Авторизация и выход из системы прошли успешно")]
    public async Task AuthorizationAndLogout_MustBe_Success()
    {
        var requestBody = new Login.Request
        {
            Email = PredefinedConstants.Admin.Email,
            Password = PredefinedConstants.Admin.Password
        };

        var loginResponse = await SessionController.Login(requestBody);
        var logoutResponse = await SessionController.Logout();
        
        var account = await Storage.FindAll<Account>()
            .Include(x => x.Tokens)
            .SingleAsync(x => x.Email == requestBody.Email);
        
        Assert.Equal(HttpStatusCode.NoContent, loginResponse.StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, logoutResponse.StatusCode);
        Assert.Empty(account.Tokens);
    }

    [Fact(DisplayName = "Авторизация и обновление сессии должны пройти успешно")]
    public async Task AuthorizationAndRefresh_MustBe_Success()
    {
        var requestBody = new Login.Request
        {
            Email = PredefinedConstants.Admin.Email,
            Password = PredefinedConstants.Admin.Password
        };

        var loginResponse = await SessionController.Login(requestBody);
        var refreshResponse = await SessionController.Refresh();
        await SessionController.Logout();
        
        Assert.Equal(HttpStatusCode.NoContent, loginResponse.StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, refreshResponse.StatusCode);
    }

    [Fact(DisplayName = "Токен обновления сессии должен быть создан и сохранен в базе данных")]
    public async Task RefreshToken_MustBe_Created()
    {
        var requestBody = new Login.Request
        {
            Email = PredefinedConstants.Admin.Email,
            Password = PredefinedConstants.Admin.Password
        };

        var loginResponse = await SessionController.Login(requestBody);
        var account = await Storage.FindAll<Account>()
            .Include(x => x.Tokens)
            .SingleAsync(x => x.Email == requestBody.Email);

        var tokens = account.Tokens
            .Where(x => x.Type == AccountTokenType.RefreshToken)
            .ToList();

        await SessionController.Logout();
        
        Assert.NotNull(loginResponse.GetCookie(JsonWebTokenConstants.Cookies.RefreshToken));
        Assert.NotEmpty(tokens);
    }
    
    [Fact(DisplayName = "При авторизации с некорректно заполненными данными, возвращает ошибку валидации")]
    public async Task AuthorizationWithIncorrectRequest_MustReturn_BadRequest()
    {
        var requestBody = new Login.Request
        {
            Email = string.Empty,
            Password = PredefinedConstants.Admin.Password
        };

        var loginResponse = await SessionController.Login(requestBody);
        Assert.Equal(HttpStatusCode.BadRequest, loginResponse.StatusCode);
    }
    
    [Fact(DisplayName = "При авторизации с ошибочными данными, возвращается отказ в доступе")]
    public async Task AuthorizationWithWrongPassword_MustReturn_Forbidden()
    {
        var requestBody = new Login.Request
        {
            Email = PredefinedConstants.Admin.Email,
            Password = "SomeWrongPassword"
        };

        var loginResponse = await SessionController.Login(requestBody);
        Assert.Equal(HttpStatusCode.Forbidden, loginResponse.StatusCode);
    }
}