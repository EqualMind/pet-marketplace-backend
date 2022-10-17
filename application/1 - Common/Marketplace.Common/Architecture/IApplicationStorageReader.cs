namespace Marketplace.Common.Architecture;

/// <summary>
/// Механизм чтения из хранилища данных приложения
/// </summary>
public interface IApplicationStorageReader
{
    /// <summary>
    /// Возвращает <see cref="IQueryable{T}"/> объект для формирования запроса к сущностям
    /// </summary>
    /// <remarks>
    /// Не выгружает навигационные свойства
    /// </remarks>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    IQueryable<TEntity> FindAll<TEntity>() where TEntity : Entity;

    /// <summary>
    /// Возвращает конкретную сущность по указанному идентификатору.
    /// Если сущности нет, возвращает Null
    /// </summary>
    /// <remarks>
    /// Не выгружает навигационные свойства
    /// </remarks>
    /// <param name="id">Идентификатор сущности</param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    Task<TEntity> FindAsync<TEntity>(Guid id) where TEntity : Entity;
}