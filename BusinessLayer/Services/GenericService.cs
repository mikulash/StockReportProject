using BusinessLayer.Exceptions;
using Infrastructure.Query;
using Infrastructure.Query.Filters;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using System.Linq.Expressions;
using DataAccessLayer.Models;

namespace BusinessLayer.Services;

public class GenericService<TEntity, TKey> : BaseService, IGenericService<TEntity, TKey> 
    where TEntity : BaseEntity<TKey>
{
    public readonly IGenericRepository<TEntity, TKey> Repository;
    public readonly IQuery<TEntity, TKey> Query;

    public GenericService(IUnitOfWork unitOfWork, IQuery<TEntity, TKey> query) : base(unitOfWork)
    {
        Repository = UnitOfWork.GetRepositoryByEntity<TEntity, TKey>();
        Query = query;
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity, bool save = true)
    {
        await Repository.AddAsync(entity);
        await SaveAsync(save);

        return entity;
    }

    public async Task<IEnumerable<TEntity>> CreateRangeAsync(TEntity[] entities, bool save = true)
    {
        await Repository.AddRangeAsync(entities);
        await SaveAsync(save);

        return entities;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, bool save = true)
    {
        Repository.Update(entity);
        await SaveAsync(save);

        return entity;
    }

    public virtual async Task<IEnumerable<TEntity>> FetchAllAsync() => await Repository.GetAllAsync();

    protected async Task<QueryResult<TEntity>> ExecuteQueryAsync(
        IFilter<TEntity> filter, 
        QueryParams? queryParams,
        params Expression<Func<TEntity, object?>>[]? includes)
    {
        Query.Filter = filter;
        Query.QueryParams = queryParams;

        Query
            .Where(Query.Filter.CreateExpression());

        var totalCount = queryParams is not null ? await Query.CountTotalAsync() : 0;

        if (includes is not null)
        {
            Query.Include(includes);
        }

        if (Query.QueryParams is not null)
        {
            Query
                .Page(Query.QueryParams.PageNumber, Query.QueryParams.PageSize)
                .SortBy(Query.QueryParams.SortParameter, Query.QueryParams.SortAscending);
        }

        var result = await Query.ExecuteAsync();
        result.TotalItemsCount = totalCount;

        return result;
    }

    public virtual async Task<QueryResult<TEntity>> FetchFilteredAsync(IFilter<TEntity> filter, QueryParams? queryParams) 
        => await ExecuteQueryAsync(filter, queryParams);

    public virtual async Task<TEntity> FindByIdAsync(TKey id)
    {
        var entity = await Repository.GetByIdAsync(id);
        if (entity is null)
        {
            throw new NoSuchEntityException<TKey>(typeof(TEntity), id);
        }
        return entity;
    }

    public virtual async Task DeleteAsync(TEntity entity, bool save = true)
    {
        Repository.Delete(entity);
        await SaveAsync(save);
    }

    public virtual async Task DeleteRangeAsync(TEntity[] entities, bool save = true)
    {
        Repository.DeleteRange(entities);
        await SaveAsync(save);
    }
}
