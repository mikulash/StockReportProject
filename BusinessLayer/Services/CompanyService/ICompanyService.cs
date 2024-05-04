using DataAccessLayer.Models;
using GenericBusinessLayer.Services;

namespace BusinessLayer.Services.CompanyService;

public interface ICompanyService : IGenericService<Company, long>
{
    Task<Company> FindByIdAllIndexRecordsAsync(long id);
}
