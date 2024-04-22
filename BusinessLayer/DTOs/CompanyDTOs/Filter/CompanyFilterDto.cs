using BusinessLayer.DTOs.BaseFilter;

namespace BusinessLayer.DTOs.CompanyDTOs.Filter;

public class CompanyFilterDto : FilterDto
{
    public string? CONTAINS_CompanyName { get; set; }
    public string? CONTAINS_Ticker { get; set; }
    public string? CONTAINS_Cusip { get; set; }
}
