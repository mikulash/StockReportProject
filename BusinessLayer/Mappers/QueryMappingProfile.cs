using AutoMapper;
using BusinessLayer.DTOs.BaseFilter;
using Infrastructure.Query;

namespace BusinessLayer.Mappers;

public class QueryMappingProfile : Profile
{
    public QueryMappingProfile()
    {
        CreateMap<FilterDto, QueryParams>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap(typeof(QueryResult<>), typeof(FilterResultDto<>));
    }
}
