using DataAccessLayer.Models;
using GenericInfrastructure.Repository;
using GenericInfrastructure.UnitOfWork;

namespace StockInfrastructure.UnitOfWork;

public interface IStockUnitOfWork : IBaseUnitOfWork
{
    IGenericRepository<Company, long> CompanyRepository { get; }
    IGenericRepository<Fund, long> FundRepository { get; }
    IGenericRepository<IndexRecord, long> IndexRecordRepository { get; }
}