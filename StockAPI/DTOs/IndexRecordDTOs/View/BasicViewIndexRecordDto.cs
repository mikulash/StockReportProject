namespace StockAPI.DTOs.IndexRecordDTOs.View;

public class BasicViewIndexRecordDto
{
    public long Id { get; set; }
    public DateOnly IssueDate { get; set; }
    public long Shares { get; set; }
    public double MarketValue { get; set; }
    public double Weight { get; set; }
}
