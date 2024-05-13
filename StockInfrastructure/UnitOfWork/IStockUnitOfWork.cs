using DataAccessLayer.Models;
using GenericInfrastructure.Query;
using GenericInfrastructure.Repository;
using GenericInfrastructure.UnitOfWork;

namespace StockInfrastructure.UnitOfWork;

public interface IStockUnitOfWork : IBaseUnitOfWork
{
    IGenericRepository<Company, long> CompanyRepository { get; init; }
    IGenericRepository<Fund, long> FundRepository { get; init; }
    IGenericRepository<IndexRecord, long> IndexRecordRepository { get; init; }
    
    IQuery<Company, long> CompanyQuery { get; init; }
    IQuery<Fund, long> FundQuery { get; init; }
    IQuery<IndexRecord, long> IndexRecordQuery { get; init; }
}