using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using LinqKit;
using Marketplace.Common.Architecture;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Marketplace.Infrastructure.Database;

public class MarketplaceContext : DbContext, IApplicationStorage
{
    public MarketplaceContext(DbContextOptions<MarketplaceContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

    /// <summary>
    /// Осуществляет очистку механизма отслеживания изменений
    /// </summary>
    protected void ClearTracking() => ChangeTracker.Clear();

    #region Application storage implementation

    public IQueryable<TEntity> FindAll<TEntity>() where TEntity : Entity
        => Set<TEntity>().IgnoreAutoIncludes().AsNoTracking();

    public Task<TEntity> FindAsync<TEntity>(Guid id) where TEntity : Entity
        => FindAll<TEntity>().SingleAsync(entity => entity.Id == id);

    public IQueryable<TEntity> ExtractEntities<TEntity>() where TEntity : Entity
        => Set<TEntity>().AsNoTracking();

    public async Task AddAsync<TEntity>(TEntity entity) where TEntity : Entity
        => await WrapAction(entity, Add);

    public async Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity
     => await WrapAction(entities, AddRange);

    public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : Entity
        => await WrapAction(entity, Update);

    public async Task UpdateRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity
        => await WrapAction(entities, UpdateRange);

    public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : Entity
        => await WrapAction(entity, Remove);

    public async Task DeleteRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity
        => await WrapAction(entities, RemoveRange);

    private async Task WrapAction<TEntity>(TEntity entity, Func<TEntity, EntityEntry<TEntity>> action) where TEntity : Entity
    {
        ClearTracking();

        var entityCache = EntityCacheManager<TEntity>.Register(entity);
        entityCache.CacheNavigationProps();

        action.Invoke(entity);
        await SaveChangesAsync();
        
        entityCache.RestoreNavigationProps();
        ClearTracking();
    }
    
    private async Task WrapAction<TEntity>(IEnumerable<TEntity> entities, Action<IEnumerable<TEntity>> action) where TEntity : Entity
    {
        ClearTracking();

        var entitiesList = entities.ToList();
        var entitiesCache = EntityCacheManager<TEntity>.Register(entitiesList);
        entitiesCache.ForEach(cache => cache.CacheNavigationProps());

        action.Invoke(entitiesList);
        await SaveChangesAsync();
        
        entitiesCache.ForEach(cache => cache.RestoreNavigationProps());
        ClearTracking();
    }

    #endregion

    #region Navigation caching

    /// <summary>
    /// Менеджер кеширования навигационных свойств сущности
    /// </summary>
    private class EntityCacheManager<TEntity> where TEntity : Entity
    {
        private readonly TEntity entity;
        private readonly Dictionary<string, object?> cache = new();

        private EntityCacheManager(TEntity entity)
        {
            this.entity = entity;
        }

        /// <summary>
        /// Закешировать и обнулить свойства зарегистрированной сущности
        /// </summary>
        public void CacheNavigationProps()
        {
            foreach (var navigationProperty in entity.GetType().GetProperties().Where(IsNavigationProperty.Compile()))
            {
                cache.Add(navigationProperty.Name, navigationProperty.GetValue(entity, null));
                navigationProperty.SetValue(entity, null);
            }
        }

        /// <summary>
        /// Вернуть свойства из кеша в зарегистрированную сущность
        /// </summary>
        public void RestoreNavigationProps()
        {
            foreach (var navigationProperty in entity.GetType().GetProperties().Where(IsNavigationProperty.Compile()))
            {
                navigationProperty.SetValue(entity, cache[navigationProperty.Name]);
            }

            cache.Clear();
        }

        /// <summary>
        /// Регистрирует сущность в менеджере кеширования навигационных свойств сущности
        /// </summary>
        public static EntityCacheManager<TEntity> Register(TEntity entity) => new(entity);

        /// <summary>
        /// Регистрирует набор сущностей в менеджере кеширования навигационных свойств сущности
        /// </summary>
        public static List<EntityCacheManager<TEntity>> Register(List<TEntity> entities) => entities.Select(Register).ToList();

        private static Expression<Func<PropertyInfo, bool>> IsNavigationProperty
            => PredicateBuilder.New<PropertyInfo>(false)
                .Or(IsEntity)
                .Or(IsEnumerableEntity)
                .Or(IsArrayEntity);

        private static Expression<Func<PropertyInfo, bool>> IsEntity
            => PredicateBuilder.New<PropertyInfo>(true)
                .And(info => info.PropertyType.IsAssignableTo(typeof(Entity)));

        private static Expression<Func<PropertyInfo, bool>> IsEnumerableEntity
            => PredicateBuilder.New<PropertyInfo>(true)
                .And(info => info.PropertyType.IsAssignableTo(typeof(IEnumerable)))
                .And(info => info.PropertyType.IsGenericType)
                .And(info => info.PropertyType.GetGenericArguments()[0].IsAssignableTo(typeof(Entity)));

        private static Expression<Func<PropertyInfo, bool>> IsArrayEntity
            => PredicateBuilder.New<PropertyInfo>(true)
                .And(info => info.PropertyType.IsArray)
                .And(info => info.PropertyType.GetElementType() != null && info.PropertyType.GetElementType()!.IsAssignableTo(typeof(Entity)));

        #endregion
    }
}