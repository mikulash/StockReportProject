using System.Linq.Expressions;

namespace Infrastructure.Repository;

public interface IGenericRepository<TEntity, TKey> where TEntity : class
{
    public string KeyName { get; }
    IQueryable<TEntity> AsQueryable();
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(params TEntity[] entities);
    void Delete(TEntity entity);
    void Update(TEntity entity);
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>>? filter = null, params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity?> GetByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includes);
    Task SaveChangesAsync();
}
