using DataAccessLayer.Models;
using GenericBusinessLayer.Services;
using GenericInfrastructure.Query;
using GenericInfrastructure.Query.Filters;
using StockInfrastructure.Query;
using StockInfrastructure.Query.Filters;

namespace StockBusinessLayer.Services.IndexRecordService;

public interface IIndexRecordService : IGenericService<IndexRecord, long>
{
    Task<QueryResult<IndexRecord>> FetchFilteredMinimalAsync(IFilter<IndexRecord> filter, QueryParams queryParams);
    Task<bool> ExistByDateAndFundNameAsync(string fundName, DateOnly date);
    Task<IEnumerable<IndexRecord>> FetchByDateAndFundNameAsync(string fundName, DateOnly date);
    Task<DateOnly?> FetchComparableOlderDateAsync(string fundName, DateOnly date);
}
