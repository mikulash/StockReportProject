using BusinessLayer.Services.IndexRecordService;
using DataAccessLayer.Models;
using GenericBusinessLayer.Facades;
using Infrastructure.Query.Filters.EntityFilters;
using StockAPI.DTOs.IndexRecordDTOs.Create;
using StockAPI.DTOs.IndexRecordDTOs.Update;
using StockAPI.DTOs.IndexRecordDTOs.View;

namespace BusinessLayer.Facades.IndexRecordFacade;

public interface IIndexRecordFacade : IGenericFacade<IndexRecord, long, IIndexRecordService, 
    CreateIndexRecordDto, UpdateIndexRecordDto, DetailedViewIndexRecordDto, DetailedViewIndexRecordDto, IndexRecordFilter>
{
    Task DeleteByDateAndFundAsync(string fundName, DateOnly date);
}
