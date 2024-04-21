using BusinessLayer.DTOs.BaseFilter;

namespace BusinessLayer.DTOs.FundDTO;

public class FundFilterDto : FilterDto
{
    public string? CONTAINS_FundName { get; set; }
}
