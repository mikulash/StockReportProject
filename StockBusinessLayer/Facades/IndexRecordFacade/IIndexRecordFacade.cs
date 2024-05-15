using DataAccessLayer.Models;
using GenericBusinessLayer.Facades;
using StockInfrastructure.Query.Filters.EntityFilters;
using StockAPI.DTOs.IndexRecordDTOs.Create;
using StockAPI.DTOs.IndexRecordDTOs.Update;
using StockAPI.DTOs.IndexRecordDTOs.View;
using StockBusinessLayer.Services.IndexRecordService;

namespace StockBusinessLayer.Facades.IndexRecordFacade;

public interface IIndexRecordFacade : IGenericFacade<IndexRecord, long, IIndexRecordService, 
    CreateIndexRecordDto, UpdateIndexRecordDto, DetailedViewIndexRecordDto, DetailedViewIndexRecordDto>
{
    Task DeleteByDateAndFundAsync(string fundName, DateOnly date);
}
