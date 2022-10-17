namespace Marketplace.Common.Architecture;

/// <summary>
/// Хранилище данных приложения
/// </summary>
/// <remarks>
/// Используется для взаимодействия с объектами <see cref="Entity"/>
/// </remarks>
public interface IApplicationStorage : IApplicationStorageReader
{
    /// <summary>
    /// Возвращает конструктор запросов к сущностям со всеми их навигационными свойствами
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    IQueryable<TEntity> ExtractEntities<TEntity>() where TEntity : Entity;

    /// <summary>
    /// Добавляет новую сущность в хранилище данных
    /// </summary>
    /// <param name="entity">Объект сущности</param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    Task AddAsync<TEntity>(TEntity entity) where TEntity : Entity;

    /// <summary>
    /// Добавляет набор сущностей в хранилище данных
    /// </summary>
    /// <param name="entities">Набор объектов сущности</param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity;

    /// <summary>
    /// Обновляет информацию об указанной сущности
    /// </summary>
    /// <param name="entity">Объект сущности</param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    Task UpdateAsync<TEntity>(TEntity entity) where TEntity : Entity;

    /// <summary>
    /// Обновляет информацию об указанных сущностях
    /// </summary>
    /// <param name="entities">Набор объектов сущности</param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    Task UpdateRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity;

    /// <summary>
    /// Удаляет указанную сущность
    /// </summary>
    /// <param name="entity">Объект сущности</param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    Task DeleteAsync<TEntity>(TEntity entity) where TEntity : Entity;

    /// <summary>
    /// Удаляет набор указанных сущностей
    /// </summary>
    /// <param name="entities">Набор объектов сущности</param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    Task DeleteRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity;
}