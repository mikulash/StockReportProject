using AutoMapper;
using GenericBusinessLayer.DTOs.BaseFilter;
using GenericInfrastructure.Query;

namespace GenericBusinessLayer.Mappers;

public class QueryMappingProfile : Profile
{
    public QueryMappingProfile()
    {
        CreateMap<FilterDto, QueryParams>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap(typeof(QueryResult<>), typeof(FilterResultDto<>));
    }
}
