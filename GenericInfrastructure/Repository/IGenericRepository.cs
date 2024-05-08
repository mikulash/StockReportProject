using System.Linq.Expressions;
using GenericDataAccessLayer.Models;

namespace GenericInfrastructure.Repository;

public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    public string KeyName { get; }
    IQueryable<TEntity> AsQueryable();
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(params TEntity[] entities);
    void Delete(TEntity entity);
    void DeleteRange(params TEntity[] entities);
    void Update(TEntity entity);
    void UpdateRange(params TEntity[] entities);
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity?> GetSingleAsync(Expression<Func<TEntity, bool>>? filter = null, params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity?> GetByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includes);
    Task SaveChangesAsync();
}
