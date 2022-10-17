using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Marketplace.Interaction.Bootstrap;

public static class SwaggerExtensions
{
    public static void AddAutoDocumentation<TGroupsList>(this IServiceCollection services, IWebHostEnvironment env)
    {
        if (env.IsProduction()) return;

        var groups = GetGroupsFrom<TGroupsList>();

        services.AddSwaggerGen(options =>
        {
            foreach (var group in groups)
            {
                options.SwaggerDoc(group, new()
                {
                    Title = $"Marketplace API Documentation ({group})",
                    Description = "Документация к Rest API серверу торговой площадки",
                    Contact = new OpenApiContact
                    {
                        Name = "EqualMind",
                        Email = "alexandr.kitsenko@yandex.ru"
                    }
                });
            }
            
            options.EnableAnnotations();
        });
    }

    public static void UseAutoDocumentation<TGroupsList>(this WebApplication app)
    {
        if (app.Environment.IsProduction()) return;

        var groups = GetGroupsFrom<TGroupsList>();
        
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var group in groups)
            {
                options.SwaggerEndpoint($"/swagger/{group.ToLower()}/swagger.json", group.ToLower());
            }
            
            options.DefaultModelsExpandDepth(-1);
        });
    }
    
    private static List<string> GetGroupsFrom<TApiGroupsTagsClass>()
        => typeof(TApiGroupsTagsClass)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(info => info.IsLiteral && !info.IsInitOnly)
            .Where(info => info.FieldType == typeof(string))
            .Select(info => info.GetValue(null) as string)
            .Select(x => x?.Trim().ToLower())
            .ToList()!;
}