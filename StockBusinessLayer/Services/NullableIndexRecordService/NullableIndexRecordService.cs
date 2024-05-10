using FileLoader.Model;
using StockAPI.DTOs.CompanyDTOs.Create;
using StockBusinessLayer.Exceptions;

namespace StockBusinessLayer.Services.NullableIndexRecordService;

public class NullableIndexRecordService : INullableIndexRecordService
{
    public List<NullableIndexRecordDto> IndexRecordList { get; set; } = [];
    public DateOnly Date { get; set; }
    public string FundName { get; set; } = string.Empty;
    public List<CreateCompanyDto> CompanyList { get; set; } = [];

    private static DateOnly ExtractDate(List<NullableIndexRecordDto> indexList)
    {
        var indexRecord = indexList.Find(rec => rec.Date is not null);
        return (indexRecord is not null 
                && indexList.TrueForAll(rec => rec.Date == indexRecord.Date)) 
            ? (DateOnly)indexRecord.Date! 
            : throw new InvalidRecordsException("Multiple date(s) detected inside provided file");
    }

    private static string ExtractFundName(List<NullableIndexRecordDto> indexList)
    {
        var indexRecord = indexList.Find(rec => rec.Fund is not null);
        if (indexRecord is null 
            || indexList.Exists(rec => rec.Fund != indexRecord.Fund))
        {
            throw new InvalidRecordsException("Multiple Funds detected inside provided file");
        }

        return indexRecord.Fund ?? string.Empty;
    }

    private static List<CreateCompanyDto> ExtractCompanies(List<NullableIndexRecordDto> indexList) =>
        indexList
            .Where(rec => rec.CUSIP is not null && rec.Company is not null)
            .Select(rec 
                => new CreateCompanyDto
                {
                    CompanyName = rec.Company ?? string.Empty, 
                    CUSIP = rec.CUSIP ?? string.Empty, 
                    Ticker = rec.Ticker ?? string.Empty
                })
            .ToList();

    private static bool HasNullSharesOrValue(NullableIndexRecordDto record) =>
        record.Shares is null || record.MarketValue is null;

    private static bool HasCompanyIdentificationNull(NullableIndexRecordDto record) =>
        record.Company is null && record.CUSIP is null;
    
    public void ApplyFilter(List<NullableIndexRecordDto> indexRecordList)
    {
        var workingCopy = new List<NullableIndexRecordDto>(indexRecordList)
            .Where(rec => !HasNullSharesOrValue(rec))
            .Where(rec => !HasCompanyIdentificationNull(rec))
            .ToList();
        
        Date = ExtractDate(workingCopy);
        FundName = ExtractFundName(workingCopy);
        CompanyList = ExtractCompanies(workingCopy);

        IndexRecordList = workingCopy;
    }
}
