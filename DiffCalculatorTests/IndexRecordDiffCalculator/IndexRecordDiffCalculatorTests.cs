using DiffCalculator.IndexRecordDiffCalculator;
using DiffCalculator.Model;
using FileLoader.Model;

namespace DiffCalculatorTests.IndexRecordDiffCalculator;

public class IndexRecordDiffCalculatorTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetIndexRecordListDiff_EmptyLists_ReturnsEmptyDiffList()
    {
        // Arrange
        var listA = new List<IndexRecordDto>();
        var listB = new List<IndexRecordDto>();
        var diffCalculator = new IndexRecordListDiffCalculator(listA, listB);

        var expected = new RecordDiffs()
        {
            DiffRecords = new List<IndexRecordDiffDto>()
        };

        // Act
        var actual = diffCalculator.GetIndexRecordListDiff();

        // Assert
        AssertIndexRecordDiffListsAreEqual(expected.DiffRecords, actual.DiffRecords);
    }

    [Test]
    public void GetIndexRecordListDiff_EmptyListB_ReturnsEmptyDiffList()
    {
        // Arrange
        var listA = new List<IndexRecordDto>()
        {
            new IndexRecordDto
            {
                CUSIP = "A1",
                Company = "Test Company 1",
                Fund = "Test Fund 1",
                Ticker = "Test Ticker 1",
                Date = new DateOnly(2024, 1, 1),
                MarketValue = 1000,
                Shares = 1000,
                Weight = 1000
            },
        };
        var listB = new List<IndexRecordDto>();
        var diffCalculator = new IndexRecordListDiffCalculator(listA, listB);

        var expected = new RecordDiffs()
        {
            DiffRecords = new List<IndexRecordDiffDto>()
        };

        // Act
        var actual = diffCalculator.GetIndexRecordListDiff();

        // Assert
        AssertIndexRecordDiffListsAreEqual(expected.DiffRecords, actual.DiffRecords);
    }

    [Test]
    public void GetIndexRecordListDiff_EmptyListA_ReturnsAllFromListBAsNew()
    {
        // Arrange
        var listA = new List<IndexRecordDto>();
        var listB = new List<IndexRecordDto>()
        {
            new IndexRecordDto
            {
                CUSIP = "A1",
                Company = "Test Company 1",
                Fund = "Test Fund 1",
                Ticker = "Test Ticker 1",
                Date = new DateOnly(2024, 1, 1),
                MarketValue = 1000,
                Shares = 1000,
                Weight = 1000
            },
        };
        var diffCalculator = new IndexRecordListDiffCalculator(listA, listB);

        var expected = new RecordDiffs()
        {
            DiffRecords = new List<IndexRecordDiffDto>()
            {
                new IndexRecordDiffDto()
                {
                    CUSIP = "A1",
                    Company = "Test Company 1",
                    Fund = "Test Fund 1",
                    Ticker = "Test Ticker 1",
                    DayDiff = new DateOnly(2024, 1, 1).DayNumber,
                    MarketValueDiff = 1000,
                    SharesDiff = 1000,
                    WeightDiff = 1000,
                    IsNew = true
                },
            }
        };

        // Act
        var actual = diffCalculator.GetIndexRecordListDiff();

        // Assert
        AssertIndexRecordDiffListsAreEqual(expected.DiffRecords, actual.DiffRecords);
    }

    [Test]
    public void GetIndexRecordListDiff_SameLists_ReturnsNoDifference()
    {
        // Arrange
        var list = new List<IndexRecordDto>()
        {
            new IndexRecordDto
            {
                CUSIP = "A1",
                Company = "Test Company 1",
                Fund = "Test Fund 1",
                Ticker = "Test Ticker 1",
                Date = new DateOnly(2024, 1, 1),
                MarketValue = 1000,
                Shares = 1000,
                Weight = 1000
            },
        };
        var diffCalculator = new IndexRecordListDiffCalculator(list, list);

        var expected = new RecordDiffs()
        {
            DiffRecords = new List<IndexRecordDiffDto>()
            {
                new IndexRecordDiffDto()
                {
                    CUSIP = "A1",
                    Company = "Test Company 1",
                    Fund = "Test Fund 1",
                    Ticker = "Test Ticker 1",
                    DayDiff = 0,
                    SharesDiff = 0,
                    SharesDiffPercentage = 0,
                    MarketValueDiff = 0,
                    WeightDiff = 0,
                    IsNew = false
                }
            }
        };

        // Act
        var actual = diffCalculator.GetIndexRecordListDiff();

        // Assert
        AssertIndexRecordDiffListsAreEqual(expected.DiffRecords, actual.DiffRecords);
    }

    [Test]
    public void GetIndexRecordListDiff_DifferentFiles_ReturnsCorrectDiff()
    {
        // Arrange
        var listA = new List<IndexRecordDto>()
        {
            new IndexRecordDto
            {
                CUSIP = "A1",
                Company = "Test Company 1",
                Fund = "Test Fund 1",
                Ticker = "Test Ticker 1",
                Date = new DateOnly(2024, 1, 1),
                MarketValue = 1000,
                Shares = 1000,
                Weight = 1000
            },
        };
        var listB = new List<IndexRecordDto>()
        {
            new IndexRecordDto
            {
                CUSIP = "A1",
                Company = "Test Company 1",
                Fund = "Test Fund 1",
                Ticker = "Test Ticker 1",
                Date = new DateOnly(2024, 1, 2),
                MarketValue = 1200,
                Shares = 1500,
                Weight = 900
            },
        };
        var diffCalculator = new IndexRecordListDiffCalculator(listA, listB);

        var expected = new RecordDiffs()
        {
            DiffRecords = new List<IndexRecordDiffDto>()
            {
                new IndexRecordDiffDto()
                {
                    CUSIP = "A1",
                    Company = "Test Company 1",
                    Fund = "Test Fund 1",
                    Ticker = "Test Ticker 1",
                    DayDiff = 1,
                    SharesDiff = 500,
                    SharesDiffPercentage = 50,
                    MarketValueDiff = 200,
                    WeightDiff = -100,
                    IsNew = false
                }
            }
        };

        // Act
        var actual = diffCalculator.GetIndexRecordListDiff();

        // Assert
        AssertIndexRecordDiffListsAreEqual(expected.DiffRecords, actual.DiffRecords);
    }

    [Test]
    public void GetIndexRecordListDiff_Decrease_ReturnsNegativeValue()
    {
        // Arrange
        var listA = new List<IndexRecordDto>()
        {
            new IndexRecordDto
            {
                CUSIP = "A1",
                Company = "Test Company 1",
                Fund = "Test Fund 1",
                Ticker = "Test Ticker 1",
                Date = new DateOnly(2024, 1, 1),
                MarketValue = 1000,
                Shares = 1500,
                Weight = 1000
            },
        };
        var listB = new List<IndexRecordDto>()
        {
            new IndexRecordDto
            {
                CUSIP = "A1",
                Company = "Test Company 1",
                Fund = "Test Fund 1",
                Ticker = "Test Ticker 1",
                Date = new DateOnly(2024, 1, 2),
                MarketValue = 1200,
                Shares = 1000,
                Weight = 900
            },
        };
        var diffCalculator = new IndexRecordListDiffCalculator(listA, listB);

        var expected = new RecordDiffs()
        {
            DiffRecords = new List<IndexRecordDiffDto>()
            {
                new IndexRecordDiffDto()
                {
                    CUSIP = "A1",
                    Company = "Test Company 1",
                    Fund = "Test Fund 1",
                    Ticker = "Test Ticker 1",
                    DayDiff = 1,
                    SharesDiff = -500,
                    SharesDiffPercentage = -(1.0 / 3) * 100,
                    MarketValueDiff = 200,
                    WeightDiff = -100,
                    IsNew = false
                }
            }
        };

        // Act
        var actual = diffCalculator.GetIndexRecordListDiff();

        // Assert
        AssertIndexRecordDiffListsAreEqual(expected.DiffRecords, actual.DiffRecords);
    }

    private void AssertIndexRecordDiffsAreEqual(IndexRecordDiffDto a, IndexRecordDiffDto b)
    {
        Assert.That(a.CUSIP, Is.EqualTo(b.CUSIP));
        Assert.That(a.Company, Is.EqualTo(b.Company));
        Assert.That(a.Fund, Is.EqualTo(b.Fund));
        Assert.That(a.Ticker, Is.EqualTo(b.Ticker));
        Assert.That(a.DayDiff, Is.EqualTo(b.DayDiff));
        Assert.That(a.MarketValueDiff, Is.EqualTo(b.MarketValueDiff));
        Assert.That(a.SharesDiff, Is.EqualTo(b.SharesDiff));
        Assert.That(a.SharesDiffPercentage, Is.EqualTo(b.SharesDiffPercentage));
        Assert.That(a.WeightDiff, Is.EqualTo(b.WeightDiff));
        Assert.That(a.IsNew, Is.EqualTo(b.IsNew));
    }

    private void AssertIndexRecordDiffListsAreEqual(List<IndexRecordDiffDto> a, List<IndexRecordDiffDto> b)
    {
        Assert.That(a.Count, Is.EqualTo(b.Count));

        for (var i = 0; i < a.Count; i++)
        {
            AssertIndexRecordDiffsAreEqual(a[i], b[i]);
        }
    }
}