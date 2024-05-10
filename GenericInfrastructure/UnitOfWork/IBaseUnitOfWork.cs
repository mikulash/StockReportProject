using GenericDataAccessLayer.Models;
using GenericInfrastructure.Repository;

namespace GenericInfrastructure.UnitOfWork;

public interface IBaseUnitOfWork : IDisposable
{
    IGenericRepository<TEntity, TKey> GetRepositoryByEntity<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
    Task CommitAsync();
    void Rollback();
}
