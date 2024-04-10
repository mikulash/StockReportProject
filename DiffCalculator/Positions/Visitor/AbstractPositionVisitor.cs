using System.Text;
using DiffCalculator.Model;

namespace DiffCalculator.Positions.Visitor;

public abstract class AbstractPositionVisitor : IVisitor
{
    public List<IndexRecordDiffDto> State { get; protected set; } = new();
    public abstract void Visit(RecordDiffs recordDiffs);
    public abstract override string ToString();

    protected string ToStringHelper(string title, Func<IndexRecordDiffDto, string> recordFormatter)
    {
        var builder = new StringBuilder(title + "\n");
        if (State.Count == 0)
        {
            builder.AppendLine("No positions");
        }
        else
        {
            State.ForEach(record => builder.AppendLine(recordFormatter(record)));
        }

        builder.AppendLine();
        return builder.ToString();
    }
}
