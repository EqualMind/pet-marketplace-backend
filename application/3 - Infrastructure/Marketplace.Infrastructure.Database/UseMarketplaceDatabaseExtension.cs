using Marketplace.Common.Architecture;
using Marketplace.Infrastructure.Database.Seeding;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Infrastructure.Database;

public static class UseMarketplaceDatabaseExtension
{
    public static void UseMarketplaceDatabase(this WebApplication app)
    {
        var prepareDatabaseTask = Task.Run(async () =>
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MarketplaceContext>();
                context.Database.Migrate();
            }

            var seeder = new Seeder(app.Configuration, app.Environment, app.Services);
            await seeder.ApplyMigrations();
        });
        
        prepareDatabaseTask.Wait();
    }

    public static async Task ApplyMigrations(this Seeder seeder)
    {
        await seeder.ApplyAsync<AddAdminAccountSeedSpecification>();
    }
}