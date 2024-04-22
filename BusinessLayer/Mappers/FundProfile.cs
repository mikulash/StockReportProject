using AutoMapper;
using BusinessLayer.DTOs.FundDTO;
using BusinessLayer.DTOs.FundDTO.Create;
using BusinessLayer.DTOs.FundDTO.Filter;
using BusinessLayer.DTOs.FundDTO.Update;
using BusinessLayer.DTOs.FundDTO.View;
using DataAccessLayer.Models;
using Infrastructure.Query.Filters.EntityFilters;

namespace BusinessLayer.Mappers;

public class FundProfile : Profile
{
    public FundProfile()
    {
        CreateMap<CreateFundDto, Fund>();
        CreateMap<UpdateFundDto, Fund>();
        CreateMap<Fund, ViewFundDto>();
        
        CreateMap<FundFilterDto, FundFilter>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); 

    }
}
