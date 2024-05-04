using BusinessLayer.DTOs.BaseFilter;
using BusinessLayer.DTOs.IndexRecordDTOs.View;

namespace BusinessLayer.DTOs.CompanyDTOs.View;

public class ViewCompanyFilteredIndexRecordDto
{
    public long Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Ticker { get; set; } = string.Empty;
    public string CUSIP { get; set; } = string.Empty;
    
    public FilterResultDto<BasicViewIndexRecordDto>? FilteredRecords { get; set; }
}
