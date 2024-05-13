using AutoMapper;
using StockBusinessLayer.Services;
using DataAccessLayer.Models;
using GenericBusinessLayer.Facades;
using GenericBusinessLayer.Services;
using StockInfrastructure.Query.Filters.EntityFilters;
using StockAPI.DTOs.CompanyDTOs.View;
using StockAPI.DTOs.FundDTO.View;
using StockAPI.DTOs.IndexRecordDTOs.Create;
using StockAPI.DTOs.IndexRecordDTOs.Update;
using StockAPI.DTOs.IndexRecordDTOs.View;
using StockBusinessLayer.Services.CompanyService;
using StockBusinessLayer.Services.IndexRecordService;

namespace StockBusinessLayer.Facades.IndexRecordFacade;

public class IndexRecordFacade 
    : GenericFacade<IndexRecord, long, IIndexRecordService, CreateIndexRecordDto, UpdateIndexRecordDto, 
            DetailedViewIndexRecordDto, DetailedViewIndexRecordDto, IndexRecordFilter>, 
        IIndexRecordFacade
{
    private readonly ICompanyService _companyService;
    private readonly IGenericService<Fund, long> _fundService;
    
    public IndexRecordFacade(IIndexRecordService service, IMapper mapper, 
        ICompanyService companyService, IGenericService<Fund, long> fundService) : base(service, mapper)
    {
        _fundService = fundService;
        _companyService = companyService;
    }

    public override async Task<DetailedViewIndexRecordDto> CreateAsync(CreateIndexRecordDto create)
    {
        var fundDto = Mapper.Map<ViewFundDto>(await _fundService.FindByIdAsync(create.FundId));
        var companyDto = Mapper.Map<BasicViewCompanyDto>(await _companyService.FindByIdAsync(create.CompanyId));
        
        var indexRecord = Mapper.Map<DetailedViewIndexRecordDto>(
            await Service.CreateAsync(Mapper.Map<IndexRecord>(create))
            );

        indexRecord.Fund = fundDto;
        indexRecord.Company = companyDto;

        return indexRecord;
    }

    public override async Task<DetailedViewIndexRecordDto> UpdateAsync(long key, UpdateIndexRecordDto update)
    {
        var record = await Service.FindByIdAsync(key);

        var newRecord = Mapper.Map<IndexRecord>(update);
        newRecord.Company = 
            record.CompanyId == update.CompanyId 
                ? record.Company 
                : await _companyService.FindByIdAsync(update.CompanyId);
        newRecord.Fund = 
            record.FundId == update.FundId 
                ? record.Fund 
                : await _fundService.FindByIdAsync(update.FundId);
        
        record.SelfUpdate(newRecord);
        
        return Mapper.Map<DetailedViewIndexRecordDto>(await Service.UpdateAsync(record));
    }

    public async Task DeleteByDateAndFundAsync(string fundName, DateOnly date)
    {
        var itemsToDelete = await Service.FetchByDateAndFundNameAsync(fundName, date);
        await Service.DeleteRangeAsync(itemsToDelete.ToArray());
    }
}
