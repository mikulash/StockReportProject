using System.Xml.Linq;
using DiffCalculator.Model;

namespace DiffCalculator.Positions.Visitor;

public interface IVisitor
{
    void Visit(RecordDiffs recordDiffs);

    string ToString();
    XElement ToHtml();
}
