using DataAccessLayer.Models;

namespace Infrastructure.Query.Filters.EntityFilters;

public class IndexRecordFilter : FilterBase<IndexRecord>
{
    protected override void SetUpSpecialLambdaExpressions()
    {
        LambdaDictionary.Add("Fund", source => source.Company!.CompanyName.Contains(Company!));
        LambdaDictionary.Add("Company", source => source.Fund!.FundName.Contains(Fund!));
    }
    
    public string? Fund { get; set; }
    public string? Company { get; set; }
    
    public double? GE_IssueDate { get; set; }
    public double? LE_IssueDate { get; set; }
    public double? GE_MarketValue { get; set; }
    public double? LE_MarketValue { get; set; }
    public double? GE_Shares { get; set; }
    public double? LE_Shares { get; set; }
}
