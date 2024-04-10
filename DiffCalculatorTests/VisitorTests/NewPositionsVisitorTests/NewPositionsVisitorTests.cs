using DiffCalculator.Model;
using DiffCalculator.Positions.Visitor;

namespace PositionsCalculationTests.NewPositionsVisitorTests;

public class NewPositionsVisitorTests
{
    [Test]
    public void Visit_WithNullDiffRecords_LeavesStateUnchanged()
    {
        var visitor = new NewPositionsVisitor();
        var recordDiffs = new RecordDiffs { DiffRecords = null };

        visitor.Visit(recordDiffs);

        Assert.That(visitor.State, Is.Empty);
    }

    [Test]
    public void Visit_WithEmptyDiffRecords_LeavesStateUnchanged()
    {
        var visitor = new NewPositionsVisitor();
        var recordDiffs = new RecordDiffs { DiffRecords = new List<IndexRecordDiffDto>() };

        visitor.Visit(recordDiffs);

        Assert.That(visitor.State, Is.Empty);
    }

    [Test]
    public void Visit_WithAllNewDiffRecords_AddsAllNewRecordsToState()
    {
        var visitor = new NewPositionsVisitor();
        var recordDiffs = new RecordDiffs
        {
            DiffRecords = new List<IndexRecordDiffDto>
            {
                new IndexRecordDiffDto { IsNew = true },
                new IndexRecordDiffDto { IsNew = true },
                new IndexRecordDiffDto { IsNew = true }
            }
        };

        visitor.Visit(recordDiffs);

        Assert.That(visitor.State, Has.Count.EqualTo(3));
        foreach (var record in visitor.State)
        {
            Assert.That(record.IsNew, Is.True);
        }
    }

    [Test]
    public void Visit_WithMixedDiffRecords_AddsOnlyNewRecordsToState()
    {
        var visitor = new NewPositionsVisitor();
        var recordDiffs = new RecordDiffs
        {
            DiffRecords = new List<IndexRecordDiffDto>
            {
                new IndexRecordDiffDto { IsNew = true },
                new IndexRecordDiffDto { IsNew = false },
                new IndexRecordDiffDto { IsNew = true },
                new IndexRecordDiffDto { IsNew = false },
                new IndexRecordDiffDto { IsNew = true }
            }
        };

        visitor.Visit(recordDiffs);

        Assert.That(visitor.State, Has.Count.EqualTo(3));
        foreach (var record in visitor.State)
        {
            Assert.That(record.IsNew, Is.True);
        }
    }
}
