using AutoMapper;
using BusinessLayer.DTOs.CompanyDTOs.Create;
using BusinessLayer.DTOs.CompanyDTOs.Filter;
using BusinessLayer.DTOs.CompanyDTOs.Update;
using BusinessLayer.DTOs.CompanyDTOs.View;
using DataAccessLayer.Models;
using Infrastructure.Query.Filters.EntityFilters;

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
