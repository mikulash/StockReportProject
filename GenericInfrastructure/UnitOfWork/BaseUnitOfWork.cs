using GenericDataAccessLayer.Models;
using GenericInfrastructure.Exceptions;
using GenericInfrastructure.Query;
using GenericInfrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace GenericInfrastructure.UnitOfWork;

public class BaseUnitOfWork<TDbContext> : IBaseUnitOfWork 
    where TDbContext : DbContext
{
    protected TDbContext DbContext { get; set; }

    public BaseUnitOfWork(TDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public IGenericRepository<TEntity, TKey> GetRepositoryByEntity<TEntity, TKey>() where TEntity : BaseEntity<TKey>
    {
        var repository = GetType()
            .GetProperties()
            .FirstOrDefault(x => x.PropertyType == typeof(IGenericRepository<TEntity, TKey>))?
            .GetValue(this);

        return repository is not null ? 
            (IGenericRepository<TEntity, TKey>) repository 
            : throw new RepositoryNotFoundException(typeof(TEntity));
    }

    public IQuery<TEntity, TKey> GetQueryByEntity<TEntity, TKey>() where TEntity : BaseEntity<TKey>
    {
        var query = GetType()
            .GetProperties()
            .FirstOrDefault(query => query.PropertyType == typeof(IQuery<TEntity, TKey>))?
            .GetValue(this);

        return query is not null
            ? (IQuery<TEntity, TKey>)query
            : throw new QueryNotFoundException(typeof(TEntity));
    }

    public async Task CommitAsync() => await DbContext.SaveChangesAsync();

    public void Rollback()
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            DbContext.Dispose();
        }
    }
}
