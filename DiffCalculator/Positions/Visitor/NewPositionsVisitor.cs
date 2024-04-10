using DiffCalculator.Model;

namespace DiffCalculator.Positions.Visitor;

public class NewPositionsVisitor : AbstractPositionVisitor
{
    public override void Visit(RecordDiffs recordDiffs)
        => State.AddRange(recordDiffs.DiffRecords?.Where(recordDiff => recordDiff.IsNew)
                          ?? new List<IndexRecordDiffDto>());

    public override string ToString()
        => ToStringHelper("New Positions:", record =>
            $"{record.CompanyCredentials} : #shares: {record.SharesDiffPercentage}%, weight: {record.WeightDiff}");
}
