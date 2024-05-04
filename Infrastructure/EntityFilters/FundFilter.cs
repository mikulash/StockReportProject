using DataAccessLayer.Models;
using GenericInfrastructure.Query.Filters;

namespace Infrastructure.Query.Filters.EntityFilters;

public class FundFilter : FilterBase<Fund>
{
    public string? CONTAINS_FundName { get; set; }
}

public class ExactFundFilter : FilterBase<Fund>
{
    public string? EQ_FundName { get; set; }
}
