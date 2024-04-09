using DiffCalculator.Model;
using FileLoader.Model;

namespace DiffCalculator.FileDiffCalculator;

public class FileDiffCalculator : IFileDiffCalculator
{
    public FileDiffCalculator()
    {
        
    }

    private IndexRecordDto? FindRecordByCusip(List<IndexRecordDto> recordList, string CUSIP)
    {
        foreach (var record in recordList)
        {
            if (record.CUSIP == CUSIP)
            {
                return record;
            }
        }
        return null;
    }
    
    private RecordDiffDto GetRecordDiff(IndexRecordDto recordA, IndexRecordDto recordB)
    {
        return new RecordDiffDto()
        {
            CUSIP = recordA.CUSIP,
            Company = recordA.Company,
            Fund = recordA.Fund,
            Ticker = recordA.Ticker,
            DayDiff = recordB.Date?.DayNumber - recordA.Date?.DayNumber,
            SharesDiff = recordB.Shares - recordA.Shares,
            MarketValueDiff = recordB.MarketValue - recordA.MarketValue,
            WeightDiff = recordB.Weight - recordA.Weight
        };
    }
    
    public List<RecordDiffDto> GetListDiff(List<IndexRecordDto> listA, List<IndexRecordDto> listB)
    {
        var diffList = new List<RecordDiffDto>();
        foreach (var recordA in listA)
        {
            var recordB = recordA.CUSIP is not null ? FindRecordByCusip(listB, recordA.CUSIP) : null;
            if (recordB is not null)
            {
                var diff = GetRecordDiff(recordA, recordB);
                diffList.Add(diff);
            }
        }
            
        return diffList;
    }
}