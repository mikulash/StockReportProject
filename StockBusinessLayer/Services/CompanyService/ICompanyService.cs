using DataAccessLayer.Models;
using GenericBusinessLayer.Services;

namespace StockBusinessLayer.Services.CompanyService;

public interface ICompanyService : IGenericService<Company, long>
{
    Task<Company> FindByIdAllIndexRecordsAsync(long id);
}
