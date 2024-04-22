using BusinessLayer.Exceptions;
using DataAccessLayer.Models;
using Infrastructure.Query;
using Infrastructure.UnitOfWork;

namespace BusinessLayer.Services.CompanyService;

public class CompanyService : GenericService<Company, long>, ICompanyService
{
    public CompanyService(IUnitOfWork unitOfWork, IQuery<Company, long> query) : base(unitOfWork, query)
    {
    }

    public async Task<Company> FindByIdAllIndexRecordsAsync(long id)
    {
        var company = await Repository.GetByIdAsync(id, comp => comp.IndexRecords!);

        return company ?? throw new NoSuchEntityException<long>(typeof(Company), id);
    }
}
