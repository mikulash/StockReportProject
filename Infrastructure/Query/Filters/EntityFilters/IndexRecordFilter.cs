﻿using DataAccessLayer.Models;

namespace Infrastructure.Query.Filters.EntityFilters;

public class IndexRecordFilter : FilterBase<IndexRecord>
{
    protected override void SetUpSpecialLambdaExpressions()
    {
        LambdaDictionary.Add("Fund", source => source.Fund!.FundName.Contains(Fund!));
        LambdaDictionary.Add("Company", source => source.Company!.CompanyName.Contains(Company!));
    }
    
    public string? Fund { get; set; }
    public string? Company { get; set; }
    
    public DateOnly? GE_IssueDate { get; set; }
    public DateOnly? LE_IssueDate { get; set; }
    public double? GE_MarketValue { get; set; }
    public double? LE_MarketValue { get; set; }
    public double? GE_Shares { get; set; }
    public double? LE_Shares { get; set; }
}
