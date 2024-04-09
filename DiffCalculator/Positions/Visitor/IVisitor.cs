using DiffCalculator.Model;

namespace DiffCalculator.Positions.Visitor;

public interface IVisitor
{
    void Visit(RecordDiffs recordDiffs);
}