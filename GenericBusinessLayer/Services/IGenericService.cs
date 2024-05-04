using GenericDataAccessLayer.Models;
using GenericInfrastructure.Query;
using GenericInfrastructure.Query.Filters;

namespace GenericBusinessLayer.Services;

public interface IGenericService<TEntity, TKey> : IBaseService where TEntity : BaseEntity<TKey>
{
    Task<TEntity> CreateAsync(TEntity entity, bool save = true);
    Task<IEnumerable<TEntity>> CreateRangeAsync(TEntity[] entities, bool save = true);
    Task<TEntity> UpdateAsync(TEntity entity, bool save = true);
    Task<IEnumerable<TEntity>> FetchAllAsync();
    Task<QueryResult<TEntity>> FetchFilteredAsync(IFilter<TEntity> filter, QueryParams? queryParams);
    Task<TEntity> FindByIdAsync(TKey id);
    Task DeleteAsync(TEntity entity, bool save = true);
    Task DeleteRangeAsync(TEntity[] entities, bool save = true);
}
