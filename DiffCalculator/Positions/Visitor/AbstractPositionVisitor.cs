using System.Globalization;
using System.Text;
using System.Xml.Linq;
using DiffCalculator.Model;

namespace DiffCalculator.Positions.Visitor;

public abstract class AbstractPositionVisitor : IVisitor
{
    public List<IndexRecordDiffDto> State { get; protected set; } = new();
    public abstract void Visit(RecordDiffs recordDiffs);
    public abstract override string ToString();
    public abstract  XElement ToHtml();

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
    
    protected XElement ToHtmlHelper(string title, string shareChangePrefix)
    {
        var mainContainer = new XElement("div");
        var heading = new XElement("h1")
        {
            Value = title
        };
        var bodyContainer = new XElement("div");
        mainContainer.Add(heading);
        mainContainer.Add(bodyContainer);
        var formattedShareChangePrefix  = string.IsNullOrEmpty(shareChangePrefix) ? string.Empty : " " + shareChangePrefix;
        if (State.Count == 0)
        {
            bodyContainer.Add(new XElement("div"){ Value = "No positions" });
        }
        else
        {
            foreach (var record in State)
            {
                var formattedRecord = new XElement("div")
                {
                    Value =
                        string.Format(CultureInfo.InvariantCulture,
                            "{0} : #shares{1}: {2}%, weight: {3:F1}",
                            record.CompanyCredentials,
                            formattedShareChangePrefix,
                            record.SharesDiffPercentage,
                            record.WeightDiff)
                };

                bodyContainer.Add(formattedRecord);
            }
        }
        return mainContainer;
    }
}
