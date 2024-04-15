using System.Globalization;
using System.Text;
using DiffCalculator.Model;

namespace DiffCalculator.Positions.Visitor;

public abstract class AbstractPositionVisitor : IVisitor
{
    public List<IndexRecordDiffDto> State { get; protected set; } = new();
    public abstract void Visit(RecordDiffs recordDiffs);
    public abstract override string ToString();

    protected string ToStringHelper(string title, string shareChangePrefix)
    {
        var builder = new StringBuilder(title + "\n");
        var formattedShareChangePrefix  = string.IsNullOrEmpty(shareChangePrefix) ? string.Empty : " " + shareChangePrefix;
        if (State.Count == 0)
        {
            builder.AppendLine("No positions");
        }
        else
        {
            foreach (var record in State)
            {
                var formattedRecord = string.Format(CultureInfo.InvariantCulture,
                    "{0} : #shares{1}: {2}%, weight: {3:F1}",
                    record.CompanyCredentials,
                    formattedShareChangePrefix,
                    record.SharesDiffPercentage,
                    record.WeightDiff);
                builder.AppendLine(formattedRecord);
            }
        }

        builder.AppendLine();
        return builder.ToString();
    }
}
