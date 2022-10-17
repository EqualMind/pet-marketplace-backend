namespace Marketplace.Common.Architecture;

/// <summary>
/// Базовый класс для всех сущностей приложения
/// </summary>
/// <remarks>
/// Используется для проектирования доменного уровня приложения
/// </remarks>
public abstract class Entity
{
    /// <summary>
    /// Идентификатор сущности
    /// </summary>
    public Guid Id { get; protected set; } = Guid.NewGuid();

    /// <summary>
    /// Дата/время создания сущности
    /// </summary>
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    /// <summary>
    /// Дата/время последнего обновления данных сущности
    /// </summary>
    public DateTime? UpdatedAt { get; protected set; }

    /// <summary>
    /// Дата/время удаления (деактивации) сущности
    /// </summary>
    public DateTime? DeletedAt { get; protected set; }
}