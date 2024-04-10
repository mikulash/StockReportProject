using DiffCalculator.Model;
using DiffCalculator.Positions.Visitor;

namespace PositionsCalculationTests.DecreasedPositionsVisitorTests;

public class DecreasedPositionsVisitorTests
{
    [Test]
    public void Visit_WithNullDiffRecords_LeavesStateUnchanged()
    {
        var visitor = new DecreasedPositionsVisitor();
        var recordDiffs = new RecordDiffs { DiffRecords = null };

        visitor.Visit(recordDiffs);

        Assert.That(visitor.State, Is.Empty);
    }

    [Test]
    public void Visit_WithEmptyDiffRecords_LeavesStateUnchanged()
    {
        var visitor = new DecreasedPositionsVisitor();
        var recordDiffs = new RecordDiffs { DiffRecords = new List<IndexRecordDiffDto>() };

        visitor.Visit(recordDiffs);

        Assert.That(visitor.State, Is.Empty);
    }

    [Test]
    public void Visit_WithNoDecreasedRecords_LeavesStateUnchanged()
    {
        var visitor = new DecreasedPositionsVisitor();
        var recordDiffs = new RecordDiffs
        {
            DiffRecords = new List<IndexRecordDiffDto>
            {
                new IndexRecordDiffDto { IsNew = true, MarketValueDiff = 10 },
                new IndexRecordDiffDto { IsNew = false, MarketValueDiff = 5 },
                new IndexRecordDiffDto { IsNew = false, MarketValueDiff = 2 }
            }
        };

        visitor.Visit(recordDiffs);

        Assert.That(visitor.State, Is.Empty);
    }

    [Test]
    public void Visit_WithDecreasedRecords_AddsOnlyDecreasedRecordsToState()
    {
        var visitor = new DecreasedPositionsVisitor();
        var recordDiffs = new RecordDiffs
        {
            DiffRecords = new List<IndexRecordDiffDto>
            {
                new IndexRecordDiffDto { IsNew = true, MarketValueDiff = -10 },
                new IndexRecordDiffDto { IsNew = false, MarketValueDiff = 5 },
                new IndexRecordDiffDto { IsNew = false, MarketValueDiff = -2 },
                new IndexRecordDiffDto { IsNew = false, MarketValueDiff = -15 }
            }
        };

        visitor.Visit(recordDiffs);

        Assert.That(visitor.State, Has.Count.EqualTo(2));
        foreach (var record in visitor.State)
        {
            Assert.Multiple(() =>
            {
                Assert.That(record.IsNew, Is.False);
                Assert.That(record.MarketValueDiff, Is.LessThan(0));
            });
        }
    }
}