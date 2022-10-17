namespace Marketplace.Common.Extensions;

public static class DateTimeExtensions
{
    /// <inheritdoc cref="DateTime.SpecifyKind"/>
    public static DateTime SpecifyKind(this DateTime @this, DateTimeKind kind) => DateTime.SpecifyKind(@this, kind);

    /// <inheritdoc cref="DateTime.SpecifyKind"/>
    public static DateTime? SpecifyKind(this DateTime? @this, DateTimeKind kind) => @this?.SpecifyKind(kind);
}