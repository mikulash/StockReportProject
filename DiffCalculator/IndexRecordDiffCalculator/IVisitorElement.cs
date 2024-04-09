using DiffCalculator.Model;
using DiffCalculator.Positions.Visitor;

namespace DiffCalculator.IndexRecordDiffCalculator;

public interface IVisitorElement
{
    public void Accept(IVisitor visitor);
}