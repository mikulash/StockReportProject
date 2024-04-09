using DiffCalculator.IndexRecordDiffCalculator;
using DiffCalculator.Positions.Visitor;

namespace DiffCalculator.Model;

public class RecordDiffs : IVisitorElement
{
    public List<IndexRecordDiffDto>? DiffRecords { get; set; }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}