using DiffCalculator.Model;

namespace DiffCalculator.Positions.Visitor;

public interface IVisitor
{
    List<IndexRecordDiffDto> Visit(RecordDiffs recordDiffs);
}