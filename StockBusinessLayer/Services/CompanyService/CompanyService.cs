using DataAccessLayer.Models;
using GenericBusinessLayer.Exceptions;
using GenericBusinessLayer.Services;
using StockInfrastructure.UnitOfWork;

namespace StockBusinessLayer.Services.CompanyService;

public class CompanyService : GenericService<Company, long, IStockUnitOfWork>, ICompanyService
{
    public CompanyService(IStockUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<Company> FindByIdAllIndexRecordsAsync(long id)
    {
        var company = await Repository.GetByIdAsync(id, comp => comp.IndexRecords!);

        return company ?? throw new NoSuchEntityException<long>(typeof(Company), id);
    }
}
