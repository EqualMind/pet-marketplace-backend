using Marketplace.Common.Architecture;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketplace.Common.Extensions;

public static class EntityTypeConfigurationExtensions
{
    /// <summary>
    /// Осуществляет базовую настройку сущности
    /// </summary>
    public static void ConfigureEntityDefaultFields<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : Entity
    {
        builder.HasKey(e => e.Id);

        builder
            .Property(e => e.CreatedAt)
            .HasConversion(x => x, x => x.SpecifyKind(DateTimeKind.Utc));

        builder
            .Property(e => e.UpdatedAt)
            .HasConversion(x => x, x => x.SpecifyKind(DateTimeKind.Utc));

        builder
            .Property(e => e.DeletedAt)
            .HasConversion(x => x, x => x.SpecifyKind(DateTimeKind.Utc));
    }
}