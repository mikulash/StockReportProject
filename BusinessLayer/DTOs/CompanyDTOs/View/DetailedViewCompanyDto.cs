using BusinessLayer.DTOs.IndexRecordDTOs;
using BusinessLayer.DTOs.IndexRecordDTOs.View;

namespace BusinessLayer.DTOs.CompanyDTOs.View;

public class DetailedViewCompanyDto
{
    public string CompanyName { get; set; } = string.Empty;
    public string Ticker { get; set; } = string.Empty;
    public string Cusip { get; set; } = string.Empty;
    public IEnumerable<BasicViewIndexRecordDto>? IndexRecords { get; set; }
}
