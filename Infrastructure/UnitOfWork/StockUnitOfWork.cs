using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Infrastructure.Exceptions;
using Infrastructure.Repository;

namespace Infrastructure.UnitOfWork;

public class StockUnitOfWork(StockDbContext dbContext) : IUnitOfWork
{
    private GenericRepository<Company, long>? _companyRepository;
    private GenericRepository<Fund, long>? _fundRepository;
    private GenericRepository<IndexRecord, long>? _indexRecordRepository;

    public IGenericRepository<Company, long> CompanyRepository => _companyRepository ??= new GenericRepository<Company, long>(dbContext);
    public IGenericRepository<Fund, long> FundRepository => _fundRepository ??= new GenericRepository<Fund, long>(dbContext);
    public IGenericRepository<IndexRecord, long> IndexRecordRepository => _indexRecordRepository ??= new GenericRepository<IndexRecord, long>(dbContext);

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
    
    public async Task CommitAsync() => await dbContext.SaveChangesAsync();

    public void Rollback()
    {
    }

    public void Dispose() => Dispose(true);

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            dbContext.Dispose();
        }
    }
}
