using Marketplace.Common.ExceptionHandling;
using Marketplace.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Common.Attributes;

/// <summary>
/// Позволяет выполнять запрос только, если <see cref="IWebHostEnvironment"/> находится в состоянии <see cref="EnvironmentList.AutoTesting"/>
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AutoTestingOnlyAttribute : ApiExplorerSettingsAttribute, IAsyncActionFilter
{
    public AutoTestingOnlyAttribute()
    {
        IgnoreApi = true;
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var env = context.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
        env.IsAutoTesting().ThrowIfFalse(new ForbiddenException());
        
        await next();
    }
}