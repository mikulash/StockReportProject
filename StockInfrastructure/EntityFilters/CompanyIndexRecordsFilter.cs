using DataAccessLayer.Models;
using GenericInfrastructure.Query.Filters;

namespace StockInfrastructure.Query.Filters.EntityFilters;

public class CompanyIndexRecordsFilter : FilterBase<IndexRecord>
{
    public long CompanyId { get; set; }
}
