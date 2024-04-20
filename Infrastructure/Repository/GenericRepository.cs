using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class GenericRepository<TEntity, TKey>(DbContext dbContext) : IGenericRepository<TEntity, TKey>
    where TEntity : class
{
    public string KeyName { get; } = "id";

    public virtual IQueryable<TEntity> AsQueryable() => dbContext.Set<TEntity>().AsQueryable();

    public virtual async Task AddAsync(TEntity entity) => await dbContext.Set<TEntity>().AddAsync(entity);

    public virtual async Task AddRangeAsync(params TEntity[] entities) 
        => await dbContext.Set<TEntity>().AddRangeAsync(entities);

    public virtual void Delete(TEntity entity) => dbContext.Set<TEntity>().Remove(entity);

    public virtual void Update(TEntity entity) => dbContext.Set<TEntity>().Update(entity);

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
