using Infrastructure.Query.Filters;
using System.Linq.Expressions;

namespace Infrastructure.Query;

public interface IQuery<TEntity, TKey> where TEntity : class
{
    IFilter<TEntity>? Filter { get; set; }
    QueryParams? QueryParams { get; set; }

    IQuery<TEntity, TKey> Include(params Expression<Func<TEntity, object?>>[] includes);
    IQuery<TEntity, TKey> Where(Expression<Func<TEntity, bool>>? filter = null);
    IQuery<TEntity, TKey> SortBy(string sortAccordingTo, bool ascending);
    IQuery<TEntity, TKey> Page(int pageToFetch, int pageSize);
    Task<int> CountTotalAsync();
    Task<QueryResult<TEntity>> ExecuteAsync();
}
