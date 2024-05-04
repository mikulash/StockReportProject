using DataAccessLayer.Models;

namespace Infrastructure.Query.Filters.EntityFilters;

public class CompanyFilter : FilterBase<Company>
{
    protected override void SetUpSpecialLambdaExpressions()
    {
    }

    public string? CONTAINS_CompanyName { get; set; }
    public string? CONTAINS_Ticker { get; set; }
    public string? CONTAINS_Cusip { get; set; }
}

public class CompanyCusipRangeFilter : FilterBase<Company>
{
    protected override void SetUpSpecialLambdaExpressions()
    {
    }
    
    public List<string>? IN_Cusip { get; set; }
}
