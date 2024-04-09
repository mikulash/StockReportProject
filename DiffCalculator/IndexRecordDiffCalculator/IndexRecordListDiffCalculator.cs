using DiffCalculator.Model;
using FileLoader.Model;

namespace DiffCalculator.IndexRecordDiffCalculator;

public class IndexRecordListDiffCalculator : IIndexRecordListDiffCalculator
{
    public IndexRecordListDiffCalculator()
    {
        
    }

    private IndexRecordDiffDto GetIndexRecordDiff(IndexRecordDto recordA, IndexRecordDto recordB)
    {
        return new IndexRecordDiffDto()
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
    
    public List<IndexRecordDiffDto> GetIndexRecordListDiff(List<IndexRecordDto> listA, List<IndexRecordDto> listB)
    {
        var diffList = new List<IndexRecordDiffDto>();
        foreach (var recordA in listA)
        {
            var recordB = listB.Find(record => record.CUSIP == recordA.CUSIP);
            if (recordB is not null)
            {
                var diff = GetIndexRecordDiff(recordA, recordB);
                diffList.Add(diff);
            }
        }
            
        return diffList;
    }
}