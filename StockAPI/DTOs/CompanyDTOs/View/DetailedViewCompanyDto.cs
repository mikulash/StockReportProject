using StockAPI.DTOs.IndexRecordDTOs.View;

namespace StockAPI.DTOs.CompanyDTOs.View;

public class DetailedViewCompanyDto
{
    public long Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Ticker { get; set; } = string.Empty;
    public string CUSIP { get; set; } = string.Empty;
    public IEnumerable<BasicViewIndexRecordDto>? IndexRecords { get; set; }
}
