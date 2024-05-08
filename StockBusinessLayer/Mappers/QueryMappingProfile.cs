using AutoMapper;
using GenericBusinessLayer.DTOs.BaseFilter;
using GenericInfrastructure.Query;
using StockInfrastructure.Query;

namespace StockBusinessLayer.Mappers;

public class QueryMappingProfile : Profile
{
    public QueryMappingProfile()
    {
        CreateMap<FilterDto, QueryParams>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap(typeof(QueryResult<>), typeof(FilterResultDto<>));
    }
}
