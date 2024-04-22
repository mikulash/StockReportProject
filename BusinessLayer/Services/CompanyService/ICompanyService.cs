using DataAccessLayer.Models;

namespace BusinessLayer.Services.CompanyService;

public interface ICompanyService : IGenericService<Company, long>
{
    Task<Company> FindByIdAllIndexRecordsAsync(long id);
}
