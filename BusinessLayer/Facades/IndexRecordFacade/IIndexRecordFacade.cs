using BusinessLayer.DTOs.IndexRecordDTOs.Create;
using BusinessLayer.DTOs.IndexRecordDTOs.Update;
using BusinessLayer.DTOs.IndexRecordDTOs.View;
using BusinessLayer.Services.IndexRecordService;
using DataAccessLayer.Models;
using Infrastructure.Query.Filters.EntityFilters;

namespace BusinessLayer.Facades.IndexRecordFacade;

public interface IIndexRecordFacade : IGenericFacade<IndexRecord, long, IIndexRecordService, 
    CreateIndexRecordDto, UpdateIndexRecordDto, DetailedViewIndexRecordDto, DetailedViewIndexRecordDto, IndexRecordFilter>
{
    Task DeleteByDateAndFundAsync(string fundName, DateOnly date);
}
