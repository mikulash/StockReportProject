using AutoMapper;
using BusinessLayer.DTOs.CompanyDTOs.View;
using BusinessLayer.DTOs.FundDTO.View;
using BusinessLayer.DTOs.IndexRecordDTOs.Create;
using BusinessLayer.DTOs.IndexRecordDTOs.Update;
using BusinessLayer.DTOs.IndexRecordDTOs.View;
using BusinessLayer.Services;
using BusinessLayer.Services.CompanyService;
using BusinessLayer.Services.IndexRecordService;
using DataAccessLayer.Models;
using Infrastructure.Query.Filters.EntityFilters;

namespace BusinessLayer.Facades.IndexRecordFacade;

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
        var itemsToDelete = 
            (await Service.FetchFilteredAsync(
                new IndexRecordFilter 
                { 
                    CONTAINS_Fund_FundName = fundName, 
                    GE_IssueDate = date, 
                    LE_IssueDate = date 
                }, null))
            .Items;

        await Service.DeleteRangeAsync(itemsToDelete.ToArray());
    }
}
