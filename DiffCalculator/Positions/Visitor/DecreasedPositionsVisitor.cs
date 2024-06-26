using System.Xml.Linq;
using DiffCalculator.Model;

namespace DiffCalculator.Positions.Visitor;

public class DecreasedPositionsVisitor : AbstractPositionVisitor
{
    public override void Visit(RecordDiffs recordDiffs)
        => State.AddRange(recordDiffs.DiffRecords?
                              .Where(recordDiff => recordDiff is { IsNew: false, MarketValueDiff: < 0 })
                          ?? new List<IndexRecordDiffDto>());

    public override string ToString()
        => ToStringHelper("Decreased Positions:", "decrease");

    public override XElement ToHtml()=> ToHtmlHelper("Decreased Positions:", "decrease");
}
