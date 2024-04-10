using DiffCalculator.Model;
using FileLoader.Model;

namespace DiffCalculator.IndexRecordDiffCalculator;

public class IndexRecordListDiffCalculator : IIndexRecordListDiffCalculator
{
    private readonly List<IndexRecordDto> _prevIndexRecordList;
    private readonly List<IndexRecordDto> _currentIndexRecordList;
    
    public IndexRecordListDiffCalculator(List<IndexRecordDto> prevPrevIndexRecordList, List<IndexRecordDto> currentCurrentIndexRecordList)
    {
        _prevIndexRecordList = prevPrevIndexRecordList;
        _currentIndexRecordList = currentCurrentIndexRecordList;
    }

    private IndexRecordDiffDto GetIndexRecordDiff(IndexRecordDto prevRecord, IndexRecordDto currentRecord)
    {
        return new IndexRecordDiffDto()
        {
            CUSIP = prevRecord.CUSIP,
            Company = prevRecord.Company,
            Fund = prevRecord.Fund,
            Ticker = prevRecord.Ticker,
            DayDiff = currentRecord.Date?.DayNumber - prevRecord.Date?.DayNumber,
            SharesDiff = currentRecord.Shares - prevRecord.Shares,
            SharesDiffPercentage = (double)(currentRecord.Shares - prevRecord.Shares) / prevRecord.Shares * 100 ,
            MarketValueDiff = currentRecord.MarketValue - prevRecord.MarketValue,
            WeightDiff = currentRecord.Weight - prevRecord.Weight,
            IsNew = false
        };
    }

    private IndexRecordDiffDto CreateNewIndexRecordDiff(IndexRecordDto record)
    {
        return new IndexRecordDiffDto()
        {
            CUSIP = record.CUSIP,
            Company = record.Company,
            Fund = record.Fund,
            Ticker = record.Ticker,
            DayDiff = record.Date?.DayNumber,
            SharesDiff = record.Shares,
            MarketValueDiff = record.MarketValue,
            WeightDiff = record.Weight,
            IsNew = true
        };
    }
    
    public RecordDiffs GetIndexRecordListDiff()
    {
        var diffList = new List<IndexRecordDiffDto>();
        var newRecordsInListCurrentIndexRecordList = new List<IndexRecordDto>(_currentIndexRecordList);
        
        foreach (var prevRecord in _prevIndexRecordList)
        {
            var currentRecord = _currentIndexRecordList.Find(record => record.CUSIP == prevRecord.CUSIP);
            if (currentRecord is not null)
            {
                diffList.Add(GetIndexRecordDiff(prevRecord, currentRecord));
                newRecordsInListCurrentIndexRecordList.RemoveAll(record => record.CUSIP == prevRecord.CUSIP);
            }
        }
        foreach (var newRecord in newRecordsInListCurrentIndexRecordList)
        {
            diffList.Add(CreateNewIndexRecordDiff(newRecord));
        }
            
        return new RecordDiffs() { DiffRecords = diffList };
    }
}