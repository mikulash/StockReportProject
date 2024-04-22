using BusinessLayer.DTOs.BaseFilter;
using BusinessLayer.DTOs.CompanyDTOs.Create;
using BusinessLayer.DTOs.CompanyDTOs.Update;
using BusinessLayer.DTOs.CompanyDTOs.View;
using BusinessLayer.Services.CompanyService;
using DataAccessLayer.Models;
using Infrastructure.Query.Filters.EntityFilters;

namespace BusinessLayer.Facades.CompanyFacade;

public interface ICompanyFacade : IGenericFacade<Company, long, ICompanyService, 
    CreateCompanyDto, UpdateCompanyDto, DetailedViewCompanyDto, BasicViewCompanyDto, CompanyFilter>
{
    public Task<ViewCompanyFilteredIndexRecordDto> FindByIdFilteredIndexRecords(long id, FilterDto filter);
}
