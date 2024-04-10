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


    [Test]
    public void ToString_HasValues_ReturnsStringWithDecreasedPositions()
    {
        var visitor = new DecreasedPositionsVisitor();
        visitor.State.AddRange(new List<IndexRecordDiffDto>
        {
            new() { Company = "Company1", Ticker = "XXX", SharesDiffPercentage = -10, WeightDiff = 0.1 },
            new() { Company = "Company2", Ticker = "XXX", SharesDiffPercentage = -5, WeightDiff = 0.2 }
        });
        var expectedHeader = "Decreased Positions:";
        var expectedLine1 = "Company1 (XXX) : #shares decrease: -10%, weight: 0.1";
        var expectedLine2 = "Company2 (XXX) : #shares decrease: -5%, weight: 0.2";

        var result = visitor.ToString();

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.StartWith(expectedHeader));
            //  regional settings can change the decimal separator
            Assert.That(result, Does.Contain(expectedLine1.Replace('.', ',')));
            Assert.That(result, Does.Contain(expectedLine2.Replace('.', ',')));
        });


    }

    [Test]
    public void ToString_HasNoValues_ReturnsEmptyString()
    {
        var visitor = new DecreasedPositionsVisitor();
        var expectedHeader = "Decreased Positions:\n";
        var expectedBody = "No positions";

        var result = visitor.ToString();

        Assert.That(result, Does.StartWith(expectedHeader));
        Assert.That(result, Does.Contain(expectedBody));
    }
}
