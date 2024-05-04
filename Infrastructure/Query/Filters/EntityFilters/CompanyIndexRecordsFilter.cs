using DataAccessLayer.Models;

namespace Infrastructure.Query.Filters.EntityFilters;

public class CompanyIndexRecordsFilter : FilterBase<IndexRecord>
{
    public long CompanyId { get; set; }
}
