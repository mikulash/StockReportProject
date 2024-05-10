namespace StockAPI.DTOs.IndexRecordDTOs.Create;

public class CreateIndexRecordDto
{
    public required long FundId { get; set; }
    public required long CompanyId { get; set; }
    public required DateOnly IssueDate { get; set; }
    public required long Shares { get; set; }
    public required double MarketValue { get; set; }
    public required double Weight { get; set; }
}
