using FileLoader.Model;
using StockAPI.DTOs.CompanyDTOs.Create;

namespace BusinessLayer.Services.NullableIndexRecordService;

public interface INullableIndexRecordService
{
    List<NullableIndexRecordDto> IndexRecordList { get; set; }
    public DateOnly Date { get; set; }
    public string FundName { get; set; }
    public List<CreateCompanyDto> CompanyList { get; set; }
    void ApplyFilter(List<NullableIndexRecordDto> indexRecordList);
}
