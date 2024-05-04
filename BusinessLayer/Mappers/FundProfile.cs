using AutoMapper;
using DataAccessLayer.Models;
using Infrastructure.Query.Filters.EntityFilters;
using StockAPI.DTOs.FundDTO.Create;
using StockAPI.DTOs.FundDTO.Filter;
using StockAPI.DTOs.FundDTO.Update;
using StockAPI.DTOs.FundDTO.View;

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
