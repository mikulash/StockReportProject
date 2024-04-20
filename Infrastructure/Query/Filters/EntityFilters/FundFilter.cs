using DataAccessLayer.Models;

namespace Infrastructure.Query.Filters.EntityFilters;

public class FundFilter : FilterBase<Fund>
{
    protected override void SetUpSpecialLambdaExpressions()
    {
    }

    public string? CONTAINS_FundName { get; set; }
}
