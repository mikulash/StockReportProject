using DataAccessLayer.Models;
using GenericBusinessLayer.DTOs.BaseFilter;
using GenericBusinessLayer.Facades;
using StockInfrastructure.Query.Filters.EntityFilters;
using StockAPI.DTOs.CompanyDTOs.Create;
using StockAPI.DTOs.CompanyDTOs.Update;
using StockAPI.DTOs.CompanyDTOs.View;
using StockBusinessLayer.Services.CompanyService;

namespace StockBusinessLayer.Facades.CompanyFacade;

public interface ICompanyFacade : IGenericFacade<Company, long, ICompanyService, 
    CreateCompanyDto, UpdateCompanyDto, DetailedViewCompanyDto, BasicViewCompanyDto, CompanyFilter>
{
    public Task<ViewCompanyFilteredIndexRecordDto> FindByIdFilteredIndexRecords(long id, FilterDto filter);
}
