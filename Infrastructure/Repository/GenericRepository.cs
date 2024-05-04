using System.Linq.Expressions;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class GenericRepository<TEntity, TKey>(StockDbContext dbContext) : IGenericRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
{
    public string KeyName { get; } = RepositoryConstants.KeyName;

    public virtual IQueryable<TEntity> AsQueryable() => dbContext.Set<TEntity>().AsQueryable();

    public virtual async Task AddAsync(TEntity entity) => await dbContext.Set<TEntity>().AddAsync(entity);

    public virtual async Task AddRangeAsync(params TEntity[] entities) 
        => await dbContext.Set<TEntity>().AddRangeAsync(entities);

    public virtual void Delete(TEntity entity) => dbContext.Set<TEntity>().Remove(entity);
    public void DeleteRange(params TEntity[] entities) => dbContext.Set<TEntity>().RemoveRange(entities);

    public virtual void Update(TEntity entity) => dbContext.Set<TEntity>().Update(entity);
    public void UpdateRange(params TEntity[] entities) => dbContext.Set<TEntity>().UpdateRange(entities);

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        params Expression<Func<TEntity, object>>[] includes) 
        => await dbContext
        .Set<TEntity>()
        .AsQueryable()
        .IncludeMultipleCheck(includes)
        .WhereCheck(filter)
        .ToListAsync();

    public virtual async Task<TEntity?> GetSingleAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        params Expression<Func<TEntity, object>>[] includes) 
        => await dbContext
        .Set<TEntity>()
        .AsQueryable()
        .IncludeMultipleCheck(includes)
        .WhereCheck(filter)
        .FirstOrDefaultAsync();

    public virtual async Task<TEntity?> GetByIdAsync(
        TKey id,
        params Expression<Func<TEntity, object>>[] includes
        )
    {
        var param = Expression.Parameter(typeof(TEntity), "source");
        var constant = Expression.Constant(id);
        var member = Expression.Property(param, KeyName);

        return await dbContext
            .Set<TEntity>()
            .AsQueryable()
            .IncludeMultipleCheck(includes)
            .FirstOrDefaultAsync(
                Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(member, constant), param));
    }

    public virtual async Task SaveChangesAsync() => await dbContext.SaveChangesAsync();
}
