using BusinessLayer.DTOs.BaseFilter;

namespace BusinessLayer.DTOs.IndexRecordDTOs.Filter;

public class IndexRecordFilterDto : FilterDto
{
    public string? Fund { get; set; }
    public string? Company { get; set; }
    
    public DateOnly? GE_IssueDate { get; set; }
    public DateOnly? LE_IssueDate { get; set; }
    public double? GE_MarketValue { get; set; }
    public double? LE_MarketValue { get; set; }
    public double? GE_Shares { get; set; }
    public double? LE_Shares { get; set; }
}