using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Common.Architecture;

/// <summary>
/// Менеджер доменного уровня
/// </summary>
public abstract class DomainManager
{
    /// <inheritdoc cref="IApplicationStorage"/>
    protected IApplicationStorage Storage { get; private set; }
    
    #region Factory initialization methods

    internal void RegisterStorage(IApplicationStorage storage) => Storage = storage;

    #endregion
}

/// <inheritdoc cref="DomainManager"/>
/// <typeparam name="TRootEntity">Тип корневой сущности</typeparam>
public abstract class DomainManager<TRootEntity> : DomainManager where TRootEntity : Entity
{
    /// <summary>
    /// Возвращает набор корневых сущностей по указанному предикату
    /// </summary>
    /// <remarks>
    /// Возвращает полные модели со всеми зависимостями для возможности редактирования
    /// </remarks>
    public virtual async Task<List<TRootEntity>> GetAllFilledAsync(Expression<Func<TRootEntity, bool>> predicate)
        => await Storage.ExtractEntities<TRootEntity>()
            .Where(predicate)
            .ToListAsync();

    /// <summary>
    /// Возвращает корневую сущность по идентификатору
    /// </summary>
    /// <remarks>
    /// Возвращает полную модель со всеми зависимостями для возможности редактирования
    /// </remarks>
    /// <param name="id">Идентификатор сущности</param>
    public virtual async Task<TRootEntity> GetFilledAsync(Guid id)
        => await Storage.ExtractEntities<TRootEntity>()
            .Where(root => root.Id == id)
            .SingleAsync();

    /// <summary>
    /// Регистрирует новый объект корневой сущности в системе
    /// </summary>
    /// <param name="entity">Объект сущности</param>
    public abstract Task CreateAsync(TRootEntity entity);
    
    /// <summary>
    /// Обновляет информацию по указанной корневой сущности в системе
    /// </summary>
    /// <param name="entity">Объект сущности</param>
    public abstract Task UpdateAsync(TRootEntity entity);

    /// <summary>
    /// Удаляет корневую сущность из системы
    /// </summary>
    /// <param name="entity">Объект сущности</param>
    public abstract Task DeleteAsync(TRootEntity entity);
} 