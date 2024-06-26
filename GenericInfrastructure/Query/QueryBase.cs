﻿using System.Linq.Expressions;
using GenericDataAccessLayer.Models;
using GenericInfrastructure.Exceptions;
using GenericInfrastructure.Query.Filters;
using GenericInfrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace GenericInfrastructure.Query;

public class QueryBase<TEntity, TKey> : IQuery<TEntity, TKey> 
    where TEntity : BaseEntity<TKey>
{
    private IQueryable<TEntity> _query;

    private IGenericRepository<TEntity, TKey> Repository {  get; set; }
    public IFilter<TEntity>? Filter { get; set; }
    public QueryParams? QueryParams { get; set; }


    public QueryBase(IGenericRepository<TEntity, TKey> repository)
    {
        Repository = repository;
        _query = repository.AsQueryable();
    }

    public async Task<QueryResult<TEntity>> ExecuteAsync()
    {
        var result = await _query.ToListAsync();

        var queryResult = new QueryResult<TEntity>()
        {
            Items = result,
            PageSize = QueryParams?.PageSize ?? PagingParameters.defaultPageSize,
            PagingEnabled = QueryParams is not null,
            PageNumber = QueryParams?.PageNumber ?? PagingParameters.defaultPageNumber
        };

        return queryResult;
    }

    public async Task<int> CountTotalAsync() => await _query.CountAsync();

    public IQuery<TEntity, TKey> Page(int pageToFetch, int pageSize)
    {
        _query = _query.Skip((pageToFetch - 1) * pageSize).Take(pageSize);

        return this;
    }

    //https://stackoverflow.com/questions/1689199/c-sharp-code-to-order-by-a-property-using-the-property-name-as-a-string/67630860#67630860
    public IQuery<TEntity, TKey> SortBy(string sortAccordingTo, bool ascending)
    {
        if (sortAccordingTo == string.Empty)
        {
            return this;
        }
        
        var param = Expression.Parameter(typeof(TEntity));
        var memberAccess = Expression.Property(param, sortAccordingTo);
        if (memberAccess == null)
        {
            throw new NoSuchPropertyException(sortAccordingTo);
        }
        var convertedMemberAccess = Expression.Convert(memberAccess, typeof(object));
        var orderPredicate = Expression.Lambda<Func<TEntity, object>>(convertedMemberAccess, param);

        _query = ascending ? _query.OrderBy(orderPredicate) : _query.OrderByDescending(orderPredicate);

        return this;
    }

    public IQuery<TEntity, TKey> Where(Expression<Func<TEntity, bool>>? filter = null)
    {
        if (filter != null)
        {
            _query = _query.Where(filter);
        }

        return this;
    }

    public void Reset() => _query = Repository.AsQueryable();

    public IQuery<TEntity, TKey> Include(params Expression<Func<TEntity, object?>>[]? includes)
    {
        if (includes != null)
        {
            _query = includes
                .Aggregate(_query, (current, include) => current.Include(include));
        }

        return this;
    }
}
