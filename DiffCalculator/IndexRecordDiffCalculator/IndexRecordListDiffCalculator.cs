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
            WeightDiff = recordB.Weight - recordA.Weight,
            IsNew = false
        };
    }

    private IndexRecordDiffDto TransferToDiffDto(IndexRecordDto recordA)
    {
        return new IndexRecordDiffDto()
        {
            CUSIP = recordA.CUSIP,
            Company = recordA.Company,
            Fund = recordA.Fund,
            Ticker = recordA.Ticker,
            DayDiff = recordA.Date?.DayNumber,
            SharesDiff = recordA.Shares,
            MarketValueDiff = recordA.MarketValue,
            WeightDiff = recordA.Weight,
            IsNew = true
        };
    }
    
    public RecordDiffs GetIndexRecordListDiff(List<IndexRecordDto> listA, List<IndexRecordDto> listB)
    {
        var diffList = new List<IndexRecordDiffDto>();
        foreach (var recordA in listA)
        {
            var recordB = listB.Find(record => record.CUSIP == recordA.CUSIP);
            if (recordB is not null)
            {
                diffList.Add(GetIndexRecordDiff(recordA, recordB));
            }
            else
            {
                diffList.Add(TransferToDiffDto(recordA));
            }
        }
            
        return new RecordDiffs() { DiffRecords = diffList};
    }
}