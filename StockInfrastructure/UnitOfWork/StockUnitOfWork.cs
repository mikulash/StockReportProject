using DataAccessLayer.Data;
using DataAccessLayer.Models;
using GenericInfrastructure.Query;
using GenericInfrastructure.Repository;
using GenericInfrastructure.UnitOfWork;

namespace StockInfrastructure.UnitOfWork;

public class StockUnitOfWork : BaseUnitOfWork<StockDbContext>, IStockUnitOfWork
{
    public StockUnitOfWork(StockDbContext dbContext, 
        IGenericRepository<Company, long> companyRepository, 
        IGenericRepository<Fund, long> fundRepository, 
        IGenericRepository<IndexRecord, long> indexRecordRepository, 
        IQuery<Company, long> companyQuery, 
        IQuery<Fund, long> fundQuery, 
        IQuery<IndexRecord, long> indexRecordQuery) 
        : base(dbContext)
    {
        CompanyRepository = companyRepository;
        FundRepository = fundRepository;
        IndexRecordRepository = indexRecordRepository;
        CompanyQuery = companyQuery;
        FundQuery = fundQuery;
        IndexRecordQuery = indexRecordQuery;
    }

    public IGenericRepository<Company, long> CompanyRepository { get; init; }
    public IGenericRepository<Fund, long> FundRepository { get; init; }
    public IGenericRepository<IndexRecord, long> IndexRecordRepository { get; init; }
    
    public IQuery<Company, long> CompanyQuery { get; init; }
    public IQuery<Fund, long> FundQuery { get; init; }
    public IQuery<IndexRecord, long> IndexRecordQuery { get; init; }
}
