using DataAccessLayer.Models;
using GenericInfrastructure.Query.Filters;

namespace StockInfrastructure.Query.Filters.EntityFilters;

public class IndexRecordFilter : FilterBase<IndexRecord>
{
    public string? CONTAINS_Fund_FundName { get; set; }
    public string? CONTAINS_Company_CompanyName { get; set; }
    
    public DateOnly? GE_IssueDate { get; set; }
    public DateOnly? LE_IssueDate { get; set; }
    public double? GE_MarketValue { get; set; }
    public double? LE_MarketValue { get; set; }
    public double? GE_Shares { get; set; }
    public double? LE_Shares { get; set; }
}

public class IndexRecordDateFilter : FilterBase<IndexRecord>
{
    public string? EQ_Fund_FundName { get; set; }
    public DateOnly? LT_IssueDate { get; set; }
}
