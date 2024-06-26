﻿using AutoMapper;
using DataAccessLayer.Models;
using GenericBusinessLayer.DTOs.BaseFilter;
using GenericBusinessLayer.Facades;
using GenericInfrastructure.Query;
using StockInfrastructure.Query;
using StockInfrastructure.Query.Filters.EntityFilters;
using StockAPI.DTOs.CompanyDTOs.Create;
using StockAPI.DTOs.CompanyDTOs.Update;
using StockAPI.DTOs.CompanyDTOs.View;
using StockAPI.DTOs.IndexRecordDTOs.View;
using StockBusinessLayer.Services.CompanyService;
using StockBusinessLayer.Services.IndexRecordService;

namespace StockBusinessLayer.Facades.CompanyFacade;

public class CompanyFacade : GenericFacade<Company, long, ICompanyService, 
    CreateCompanyDto, UpdateCompanyDto, DetailedViewCompanyDto, BasicViewCompanyDto, CompanyFilter>, ICompanyFacade
{
    private readonly IIndexRecordService _indexRecordService;
    
    public CompanyFacade(ICompanyService service, IMapper mapper, IIndexRecordService indexRecordService) : base(service, mapper)
    {
        _indexRecordService = indexRecordService;
    }

    public override async Task<DetailedViewCompanyDto> FindByIdAsync(long key) =>
        Mapper.Map<DetailedViewCompanyDto>(await Service.FindByIdAllIndexRecordsAsync(key));

    public async Task<ViewCompanyFilteredIndexRecordDto> FindByIdFilteredIndexRecords(long id, FilterDto filter)
    {
        var result = Mapper.Map<ViewCompanyFilteredIndexRecordDto>(await Service.FindByIdAsync(id));
        result.FilteredRecords = 
            Mapper.Map<FilterResultDto<BasicViewIndexRecordDto>>(
                await _indexRecordService.FetchFilteredMinimalAsync(
                    new CompanyIndexRecordsFilter { CompanyId = id }, 
                    Mapper.Map<QueryParams>(filter)
                    )
                );
        
        return result;
    }
}
