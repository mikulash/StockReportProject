using DataAccessLayer.Models;
using Infrastructure.Query;
using Infrastructure.Query.Filters;

namespace BusinessLayer.Services.IndexRecordService;

public interface IIndexRecordService : IGenericService<IndexRecord, long>
{
    Task<QueryResult<IndexRecord>> FetchFilteredMinimalAsync(IFilter<IndexRecord> filter, QueryParams queryParams);   
}
