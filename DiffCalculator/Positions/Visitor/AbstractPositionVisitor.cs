using DiffCalculator.Model;

namespace DiffCalculator.Positions.Visitor;

public abstract class AbstractPositionVisitor: IVisitor
{
    public List<IndexRecordDiffDto> State { get; protected set; } = new ();
    public abstract void Visit(RecordDiffs recordDiffs);
    public abstract override string ToString();
}
