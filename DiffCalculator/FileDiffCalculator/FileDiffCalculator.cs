using DiffCalculator.Model;
using FileLoader.Model;

namespace DiffCalculator.FileDiffCalculator;

public class FileDiffCalculator : IFileDiffCalculator
{
    public FileDiffCalculator()
    {
        
    }

    // TODO: implementation
    public IndexRecordDto FindRecordByCusip(List<IndexRecordDto> recordList, string CUSIP)
    {
        return new IndexRecordDto();
    }
    
    // TODO: implementation
    public RecordDiffDto GetRecordDiff(IndexRecordDto recordA, IndexRecordDto recordB)
    {
        return new RecordDiffDto();
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