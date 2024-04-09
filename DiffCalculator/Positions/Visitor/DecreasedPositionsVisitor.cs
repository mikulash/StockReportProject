using DiffCalculator.Model;

namespace DiffCalculator.Positions.Visitor;

public class DecreasedPositionsVisitor: AbstractPositionVisitor
{
    public override void Visit(RecordDiffs recordDiffs) 
        => State.AddRange(recordDiffs.DiffRecords?
                              .Where(recordDiff => recordDiff is { IsNew: false, MarketValueDiff: < 0 }) 
                          ?? new List<IndexRecordDiffDto>());
}