using AutoMapper;
using BusinessLayer.DTOs.BaseFilter;
using BusinessLayer.DTOs.CompanyDTOs.Create;
using BusinessLayer.DTOs.CompanyDTOs.Update;
using BusinessLayer.DTOs.CompanyDTOs.View;
using BusinessLayer.DTOs.IndexRecordDTOs.View;
using BusinessLayer.Services.CompanyService;
using BusinessLayer.Services.IndexRecordService;
using DataAccessLayer.Models;
using Infrastructure.Query;
using Infrastructure.Query.Filters.EntityFilters;

namespace BusinessLayer.Facades.CompanyFacade;

public class CompanyFacade : GenericFacade<Company, long, ICompanyService, 
    CreateCompanyDto, UpdateCompanyDto, DetailedViewCompanyDto, BasicViewCompanyDto, CompanyFilter>, ICompanyFacade
{
    private IIndexRecordService _indexRecordService;
    
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
