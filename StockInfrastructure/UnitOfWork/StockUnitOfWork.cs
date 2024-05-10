using DataAccessLayer.Data;
using DataAccessLayer.Models;
using GenericInfrastructure.Repository;
using GenericInfrastructure.UnitOfWork;

namespace StockInfrastructure.UnitOfWork;

public class StockUnitOfWork : BaseUnitOfWork<StockDbContext>, IStockUnitOfWork
{
    public StockUnitOfWork(StockDbContext dbContext, 
        IGenericRepository<Company, long> companyRepository, 
        IGenericRepository<Fund, long> fundRepository, 
        IGenericRepository<IndexRecord, long> indexRecordRepository) 
        : base(dbContext)
    {
        CompanyRepository = companyRepository;
        FundRepository = fundRepository;
        IndexRecordRepository = indexRecordRepository;
    }

    public IGenericRepository<Company, long> CompanyRepository { get; init; }
    public IGenericRepository<Fund, long> FundRepository { get; init; }
    public IGenericRepository<IndexRecord, long> IndexRecordRepository { get; init; }
}
