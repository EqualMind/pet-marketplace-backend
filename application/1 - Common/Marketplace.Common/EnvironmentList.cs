using Microsoft.Extensions.Hosting;

namespace Marketplace.Common;

/// <summary>
/// Расширенный список значений состояния среды <see cref="Environments"/>
/// </summary>
public static class EnvironmentList
{
    /// <summary>
    /// Состояние среды "В разработке"
    /// </summary>
    public static string Development => Environments.Development;

    /// <summary>
    /// Состояние среды "Автотестирование"
    /// </summary>
    public static string AutoTesting => "AutoTesting";
    
    /// <summary>
    /// Состояние среды "Пре-релиз"
    /// </summary>
    public static string Staging => Environments.Staging;
    
    /// <summary>
    /// Состояние среды "Релиз"
    /// </summary>
    public static string Production => Environments.Production;
}