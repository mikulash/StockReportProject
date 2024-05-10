using StockAPI.DTOs.CompanyDTOs.View;
using StockAPI.DTOs.FundDTO.View;

namespace StockAPI.DTOs.IndexRecordDTOs.View;

public class DetailedViewIndexRecordDto
{
    public long Id { get; set; }
    public ViewFundDto? Fund { get; set; }
    public BasicViewCompanyDto? Company { get; set; }
    public DateOnly IssueDate { get; set; }
    public long Shares { get; set; }
    public double MarketValue { get; set; }
    public double Weight { get; set; }
}
