using System.Linq.Expressions;
using GenericDataAccessLayer.Models;
using GenericInfrastructure.Query.Filters;

namespace GenericInfrastructure.Query;

public interface IQuery<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    IFilter<TEntity>? Filter { get; set; }
    QueryParams? QueryParams { get; set; }
    void Reset();
    IQuery<TEntity, TKey> Include(params Expression<Func<TEntity, object?>>[] includes);
    IQuery<TEntity, TKey> Where(Expression<Func<TEntity, bool>>? filter = null);
    IQuery<TEntity, TKey> SortBy(string sortAccordingTo, bool ascending);
    IQuery<TEntity, TKey> Page(int pageToFetch, int pageSize);
    Task<int> CountTotalAsync();
    Task<QueryResult<TEntity>> ExecuteAsync();
}
