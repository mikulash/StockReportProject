using BusinessLayer.DTOs.BaseFilter;

namespace BusinessLayer.DTOs.FundDTO.Filter;

public class FundFilterDto : FilterDto
{
    public string? CONTAINS_FundName { get; set; }
}
