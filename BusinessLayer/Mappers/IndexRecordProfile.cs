using AutoMapper;
using BusinessLayer.DTOs.IndexRecordDTOs.Create;
using BusinessLayer.DTOs.IndexRecordDTOs.Filter;
using BusinessLayer.DTOs.IndexRecordDTOs.Update;
using BusinessLayer.DTOs.IndexRecordDTOs.View;
using DataAccessLayer.Models;
using FileLoader.Model;
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

        CreateMap<IndexRecord, DiffCalculator.Model.IndexRecordDto>()
            .ForMember(dto => dto.Date, opts => opts.MapFrom(rec => rec.IssueDate))
            .ForMember(dto => dto.Company, opts => opts.MapFrom(rec => rec.Company!.CompanyName))
            .ForMember(dto => dto.Ticker, opts => opts.MapFrom(rec => rec.Company!.Ticker))
            .ForMember(dto => dto.CUSIP, opts => opts.MapFrom(rec => rec.Company!.CUSIP))
            .ForMember(dto => dto.Fund, opts => opts.MapFrom(rec => rec.Fund!.FundName));
        
        CreateMap<IndexRecordFilterDto, IndexRecordFilter>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<NullableIndexRecordDto, IndexRecord>()
            .ForMember(mem => mem.CompanyId, opt => opt.Ignore())
            .ForMember(mem => mem.Company, opt => opt.Ignore())
            .ForMember(mem => mem.FundId, opt => opt.Ignore())
            .ForMember(mem => mem.Fund, opt => opt.Ignore())
            .ForMember(mem => mem.IssueDate, opt => opt.Ignore());
    }
}