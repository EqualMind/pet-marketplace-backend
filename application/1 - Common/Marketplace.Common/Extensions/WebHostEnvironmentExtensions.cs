using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Marketplace.Common.Extensions;

public static class WebHostEnvironmentExtensions
{
    /// <summary>
    /// Среда в состоянии <see cref="EnvironmentList.AutoTesting"/>
    /// </summary>
    public static bool IsAutoTesting(this IWebHostEnvironment env) => env.IsEnvironment(EnvironmentList.AutoTesting);
}