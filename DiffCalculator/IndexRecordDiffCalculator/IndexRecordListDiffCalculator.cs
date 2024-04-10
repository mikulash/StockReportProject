using DiffCalculator.Model;
using FileLoader.Model;

namespace DiffCalculator.IndexRecordDiffCalculator;

public class IndexRecordListDiffCalculator : IIndexRecordListDiffCalculator
{
    private readonly List<IndexRecordDto> _indexRecordListA;
    private readonly List<IndexRecordDto> _indexRecordListB;
    
    public IndexRecordListDiffCalculator(List<IndexRecordDto> indexRecordListA, List<IndexRecordDto> indexRecordListB)
    {
        _indexRecordListA = indexRecordListA;
        _indexRecordListB = indexRecordListB;
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
            SharesDiffPercentage =  (recordB.Shares - recordA.Shares) / recordA.Shares * 100 ,
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
    
    public RecordDiffs GetIndexRecordListDiff()
    {
        var diffList = new List<IndexRecordDiffDto>();
        var newRecordsInListB = new List<IndexRecordDto>(_indexRecordListB);
        
        foreach (var recordA in _indexRecordListA)
        {
            var recordB = _indexRecordListB.Find(record => record.CUSIP == recordA.CUSIP);
            if (recordB is not null)
            {
                diffList.Add(GetIndexRecordDiff(recordA, recordB));
                newRecordsInListB.RemoveAll(record => record.CUSIP == recordA.CUSIP);
            }
        }
        foreach (var recordB in newRecordsInListB)
        {
            diffList.Add(TransferToDiffDto(recordB));
        }
            
        return new RecordDiffs() { DiffRecords = diffList };
    }
}