using DiffCalculator.Model;
using FileLoader.Model;

namespace DiffCalculator.FileDiffCalculator;

public class FileDiffCalculator : IFileDiffCalculator
{
    public FileDiffCalculator()
    {
        
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
            var recordB = listB.Find(record => record.CUSIP == recordA.CUSIP);
            if (recordB is not null)
            {
                var diff = GetRecordDiff(recordA, recordB);
                diffList.Add(diff);
            }
        }
            
        return diffList;
    }
}