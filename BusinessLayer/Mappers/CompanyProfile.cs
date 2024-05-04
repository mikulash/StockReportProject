using AutoMapper;
using DataAccessLayer.Models;
using Infrastructure.Query.Filters.EntityFilters;
using StockAPI.DTOs.CompanyDTOs.Create;
using StockAPI.DTOs.CompanyDTOs.Filter;
using StockAPI.DTOs.CompanyDTOs.Update;
using StockAPI.DTOs.CompanyDTOs.View;

namespace BusinessLayer.Mappers;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<CreateCompanyDto, Company>();
        CreateMap<UpdateCompanyDto, Company>();
        CreateMap<Company, BasicViewCompanyDto>();
        CreateMap<Company, DetailedViewCompanyDto>();
        CreateMap<Company, ViewCompanyFilteredIndexRecordDto>()
            .ForMember(dest => dest.FilteredRecords, opt => opt.Ignore());
        
        CreateMap<CompanyFilterDto, CompanyFilter>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
