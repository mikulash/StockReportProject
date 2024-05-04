using DataAccessLayer.Models;
using Infrastructure.Query;
using Infrastructure.Query.Filters;

namespace BusinessLayer.Services.IndexRecordService;

public interface IIndexRecordService : IGenericService<IndexRecord, long>
{
    Task<QueryResult<IndexRecord>> FetchFilteredMinimalAsync(IFilter<IndexRecord> filter, QueryParams queryParams);

    Task<IEnumerable<IndexRecord>> FetchByDateAndFundNameAsync(string fundName, DateOnly date);
    Task<DateOnly?> FetchComparableOlderDateAsync(string fundName, DateOnly date);
}
