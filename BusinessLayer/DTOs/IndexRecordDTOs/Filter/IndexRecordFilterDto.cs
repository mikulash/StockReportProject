namespace BusinessLayer.DTOs.IndexRecordDTOs.Filter;

public class IndexRecordFilterDto
{
    public string? Fund { get; set; }
    public string? Company { get; set; }
    
    public double? GE_IssueDate { get; set; }
    public double? LE_IssueDate { get; set; }
    public double? GE_MarketValue { get; set; }
    public double? LE_MarketValue { get; set; }
    public double? GE_Shares { get; set; }
    public double? LE_Shares { get; set; }
}