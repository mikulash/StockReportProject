using DiffCalculator.Model;

namespace DiffCalculator.Positions.Visitor;

public class NewPositionsVisitor: AbstractPositionVisitor
{
    public override void Visit(RecordDiffs recordDiffs)
        => State.AddRange(recordDiffs.DiffRecords?.Where(recordDiff => recordDiff.IsNew)
                          ?? new List<IndexRecordDiffDto>());

    public override string ToString()
    {
        var retval =  "New Positions:\n";
        State.ForEach(record => retval += $"Company: {record.CompanyCredentials} : #shares: {record.SharesDiffPercentage}%, weight: {record.WeightDiff}\n");
        retval += "\n";
        return retval;
    }
}
