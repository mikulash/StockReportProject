using DiffCalculator.Model;

namespace DiffCalculator.Positions.Visitor;

public class NewPositionVisitor: IVisitor
{
    public List<IndexRecordDiffDto> Visit(RecordDiffs recordDiffs)
    {
        List<IndexRecordDiffDto> result = new List<IndexRecordDiffDto>();
        if (recordDiffs.DiffRecords == null)
        {
            return result;
        } 
        foreach (var recordDiff in recordDiffs.DiffRecords)
        { 
            if (recordDiff.IsNew)
            {
                result.Add(recordDiff);
            }
        }

        return result;
    }
  
}