using DataAccessLayer.Models;
using GenericInfrastructure.Query.Filters;

namespace Infrastructure.Query.Filters.EntityFilters;

public class CompanyIndexRecordsFilter : FilterBase<IndexRecord>
{
    public long CompanyId { get; set; }
}
