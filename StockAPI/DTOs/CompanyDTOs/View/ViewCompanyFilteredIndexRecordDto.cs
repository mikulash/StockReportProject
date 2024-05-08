using GenericBusinessLayer.DTOs.BaseFilter;
using StockAPI.DTOs.IndexRecordDTOs.View;

namespace StockAPI.DTOs.CompanyDTOs.View;

public class ViewCompanyFilteredIndexRecordDto
{
    public long Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Ticker { get; set; } = string.Empty;
    public string CUSIP { get; set; } = string.Empty;
    
    public FilterResultDto<BasicViewIndexRecordDto>? FilteredRecords { get; set; }
}
