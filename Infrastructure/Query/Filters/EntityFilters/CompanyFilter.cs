﻿using DataAccessLayer.Models;

namespace Infrastructure.Query.Filters.EntityFilters;

public class CompanyFilter : FilterBase<Company>
{
    public string? CONTAINS_CompanyName { get; set; }
    public string? CONTAINS_Ticker { get; set; }
    public string? CONTAINS_CUSIP { get; set; }
}

public class CompanyCusipRangeFilter : FilterBase<Company>
{
    public List<string>? IN_CUSIP { get; set; }
}
