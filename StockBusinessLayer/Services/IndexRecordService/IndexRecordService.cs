using DataAccessLayer.Models;
using GenericBusinessLayer.Services;
using GenericInfrastructure.Query;
using GenericInfrastructure.Query.Filters;
using StockInfrastructure.Query.Filters.EntityFilters;
using StockInfrastructure.UnitOfWork;

namespace StockBusinessLayer.Services.IndexRecordService;

public class IndexRecordService : GenericService<IndexRecord, long, IStockUnitOfWork>, IIndexRecordService
{
    public IndexRecordService(IStockUnitOfWork unitOfWork, IQuery<IndexRecord, long> query) : base(unitOfWork, query)
    {
    }

    public override async Task<IEnumerable<IndexRecord>> FetchAllAsync() =>
        await Repository.GetAllAsync(null, rec => rec.Company!, rec => rec.Fund!);

    public async Task<QueryResult<IndexRecord>> FetchFilteredMinimalAsync(IFilter<IndexRecord> filter, QueryParams queryParams) 
        => await ExecuteQueryAsync(filter, queryParams);

    public async Task<IEnumerable<IndexRecord>> FetchByDateAndFundNameAsync(string fundName, DateOnly date)
        => (await FetchFilteredAsync(new IndexRecordFilter
            {
                CONTAINS_Fund_FundName = fundName,
                GE_IssueDate = date,
                LE_IssueDate = date
            }, null))
            .Items;

    public async Task<DateOnly?> FetchComparableOlderDateAsync(string fundName, DateOnly date)
    {
        var filter = new IndexRecordDateFilter { LT_IssueDate = date, EQ_Fund_FundName = fundName };
        var queryParams = new QueryParams
            { PageNumber = 1, PageSize = 1, SortAscending = false, SortParameter = "IssueDate" };

        return (await ExecuteQueryAsync(filter, queryParams, 
                rec => rec.Fund, rec => rec.Company))
            .Items.FirstOrDefault()?.IssueDate;
    }

    public override async Task<QueryResult<IndexRecord>> FetchFilteredAsync(IFilter<IndexRecord> filter,
        QueryParams? queryParams)
        => await ExecuteQueryAsync(
            filter, queryParams,
            rec => rec.Company,
            rec => rec.Fund);
}
