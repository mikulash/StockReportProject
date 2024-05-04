using DataAccessLayer.Models;
using Infrastructure.Query;
using Infrastructure.Query.Filters;
using Infrastructure.UnitOfWork;

namespace BusinessLayer.Services.IndexRecordService;

public class IndexRecordService : GenericService<IndexRecord, long>, IIndexRecordService
{
    public IndexRecordService(IUnitOfWork unitOfWork, IQuery<IndexRecord, long> query) : base(unitOfWork, query)
    {
    }

    public async Task<QueryResult<IndexRecord>> FetchFilteredMinimalAsync(IFilter<IndexRecord> filter, QueryParams queryParams) 
        => await ExecuteQueryAsync(filter, queryParams);

    public override async Task<QueryResult<IndexRecord>> FetchFilteredAsync(IFilter<IndexRecord> filter,
        QueryParams? queryParams)
        => await ExecuteQueryAsync(
            filter, queryParams, 
            rec => rec.Company, 
            rec => rec.Fund);
}
