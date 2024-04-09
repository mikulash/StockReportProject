using DiffCalculator.Positions.Visitor;

namespace DiffCalculator.Model;

public class RecordDiffs
{
    public List<IndexRecordDiffDto>? DiffRecords { get; set; }

    public List<IndexRecordDiffDto> Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
    }
}