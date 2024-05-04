using DataAccessLayer.Models;
using Infrastructure.Repository;

namespace Infrastructure.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Company, long> CompanyRepository { get; }
    IGenericRepository<Fund, long> FundRepository { get; }
    IGenericRepository<IndexRecord, long> IndexRecordRepository { get; }
    
    IGenericRepository<TEntity, TKey> GetRepositoryByEntity<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
    Task CommitAsync();
    void Rollback();
}
