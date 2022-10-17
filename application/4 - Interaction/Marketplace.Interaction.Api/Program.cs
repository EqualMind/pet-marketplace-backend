using Marketplace.Interaction.Bootstrap;

namespace Marketplace.Interaction.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddMarketplaceServices(typeof(Program).Assembly);

        var app = builder.Build();

        app.UseMarketplaceApiMiddlewares();
        app.Run();
    }
}
