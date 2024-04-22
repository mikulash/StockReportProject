using AutoMapper;
using BusinessLayer.DTOs.IndexRecordDTOs.Create;
using BusinessLayer.DTOs.IndexRecordDTOs.Filter;
using BusinessLayer.DTOs.IndexRecordDTOs.Update;
using BusinessLayer.DTOs.IndexRecordDTOs.View;
using DataAccessLayer.Models;
using Infrastructure.Query.Filters.EntityFilters;

namespace BusinessLayer.Mappers;

public class IndexRecordProfile : Profile
{
    public IndexRecordProfile()
    {
        CreateMap<CreateIndexRecordDto, IndexRecord>();
        CreateMap<UpdateIndexRecordDto, IndexRecord>();
        CreateMap<IndexRecord, DetailedViewIndexRecordDto>();
        CreateMap<IndexRecord, DetailedViewIndexRecordDto>();
        CreateMap<IndexRecord, BasicViewIndexRecordDto>();
        
        CreateMap<IndexRecordFilterDto, IndexRecordFilter>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}