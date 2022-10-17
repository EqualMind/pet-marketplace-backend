using Marketplace.Common.Extensions;
using Marketplace.Infrastructure.Database;
using Marketplace.Interaction.Common;
using Microsoft.AspNetCore.Builder;

namespace Marketplace.Interaction.Bootstrap;

public static class UseMarketplaceApiMiddlewaresExtension
{
    /// <summary>
    /// Осуществляет регистрацию промежуточного ПО для торговой площадки
    /// </summary>
    public static void UseMarketplaceApiMiddlewares(this WebApplication app)
    {
        app.UseMarketplaceDatabase();
        app.UseAutoDocumentation<ApiGroups>();
        app.UseHttpsRedirection();
        app.UseHttpExceptionHandlingMiddleware();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
}