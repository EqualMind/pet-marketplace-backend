using Marketplace.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Infrastructure.Database;

public static class AddMarketplaceDatabaseExtension
{
    private static readonly Lazy<SqliteConnection> SqliteConnection = new(() =>
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        return connection;
    });
    
    /// <summary>
    /// Осуществляет подключение базы данных торговой площадки в приложении
    /// </summary>
    public static void AddMarketplaceDatabase(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
    {
        if (env.IsAutoTesting())
            services.AddDbContext<MarketplaceContext>(options =>
            {
                options.UseSqlite(SqliteConnection.Value);
                options.ConfigureWarnings(x => x.Ignore(RelationalEventId.AmbientTransactionWarning));
            });
        else
            services.AddDbContext<MarketplaceContext>(options => options.UseNpgsql(config.GetConnectionString("Npgsql")));
        
        services.AddAsApplicationStorageImplementation<MarketplaceContext>();
    }
}