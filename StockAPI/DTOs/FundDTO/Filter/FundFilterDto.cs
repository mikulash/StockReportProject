using GenericBusinessLayer.DTOs.BaseFilter;

namespace StockAPI.DTOs.FundDTO.Filter;

public class FundFilterDto : FilterDto
{
    public string? CONTAINS_FundName { get; set; }
}
