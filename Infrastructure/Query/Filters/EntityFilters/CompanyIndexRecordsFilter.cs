using DataAccessLayer.Models;

namespace Infrastructure.Query.Filters.EntityFilters;

public class CompanyIndexRecordsFilter : FilterBase<IndexRecord>
{
    protected override void SetUpSpecialLambdaExpressions()
    {
    }
    
    public long CompanyId { get; set; }
}
