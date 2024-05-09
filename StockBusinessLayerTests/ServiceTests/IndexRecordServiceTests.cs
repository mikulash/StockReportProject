using System.Linq.Expressions;
using DataAccessLayer.Models;
using GenericBusinessLayer.Exceptions;
using GenericInfrastructure.Query;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StockBusinessLayer.Services.IndexRecordService;
using StockInfrastructure.Query.Filters.EntityFilters;
using TestUtilities;

namespace StockBusinessLayerTests.ServiceTests;

public class IndexRecordServiceTests : ServiceTestBase<IndexRecord>
{
    protected override IIndexRecordService GetService()
    {
        var serviceProvider = CreateServiceProvider();

        using var scope = serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IIndexRecordService>();
    }

    private static void AddFundsAndCompanies(List<IndexRecord> indexRecords)
    {
        foreach (var item in indexRecords)
        {
            item.Company = TestDataInitializer.GetTestCompanies().First(x => x.Id.Equals(item.CompanyId));
            item.Fund = TestDataInitializer.GetTestFunds().First(x => x.Id.Equals(item.FundId));
        }
    }
    
    [Test]
    public async Task CreateAsync_NewIndexRecordCorrectFormat_ReturnsCreatedIndexRecord()
    {
        // arrange
        var expected = TestDataInitializer.GetTestIndexRecord();
        
        MockedRepository
            .Setup(mock => mock.AddAsync(It.IsAny<IndexRecord>()))
            .Returns(Task.FromResult(expected))
            .Verifiable();
        
        // act
        var indexRecordService = GetService();
        var actual = await indexRecordService.CreateAsync(expected);
            
        // assert
        Assert.NotNull(actual);
        Assert.That(actual, Is.EqualTo(expected));
        
        MockedRepository.Verify(repo => repo.AddAsync(It.IsAny<IndexRecord>()));
        MockedUoW.Verify(uow => uow.CommitAsync());
    }

    [Test]
    public async Task CreateRangeAsync_NewIndexRecordsInCorrectFormat_ReturnsCreatedIndexRecords()
    {
        // arrange
        var expected = TestDataInitializer.GetTestIndexRecords().ToArray();

        MockedRepository
            .Setup(mock => mock.AddRangeAsync(It.IsAny<IndexRecord[]>()))
            .Returns(Task.FromResult(expected))
            .Verifiable();
        
        // act
        var indexRecordService = GetService();
        var actual = (await indexRecordService.CreateRangeAsync(expected)).ToArray();
            
        // assert
        Assert.NotNull(actual);
        Assert.That(actual.Length, Is.EqualTo(expected.Length));
        Assert.That(actual, Is.EquivalentTo(expected));
        
        MockedRepository.Verify(repo => repo.AddRangeAsync(It.IsAny<IndexRecord[]>()));
        MockedUoW.Verify(uow => uow.CommitAsync());
    }

    [Test]
    public async Task UpdateAsync_NewIndexRecordName_ReturnsUpdatedIndexRecord()
    {
        // arrange
        var newDate = new DateOnly(2024, 3, 3);
        var newMarketValue = 10.1032;
        
        var expected = TestDataInitializer.GetTestIndexRecord();
        expected.IssueDate = newDate;
        expected.MarketValue = newMarketValue;
        
        MockedRepository
            .Setup(mock => mock.Update(It.IsAny<IndexRecord>()))
            .Verifiable();
        
        // act
        var indexRecordService = GetService();
        var actual = await indexRecordService.UpdateAsync(expected);
            
        // assert
        Assert.NotNull(actual);
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(actual.IssueDate, Is.EqualTo(newDate));
        Assert.That(actual.MarketValue, Is.EqualTo(newMarketValue));
        
        MockedRepository.Verify(repo => repo.Update(It.IsAny<IndexRecord>()));
        MockedUoW.Verify(uow => uow.CommitAsync());
    }

    [Test]
    public async Task FetchAllAsync_NoRestrictions_ReturnsAllIndexRecords()
    {
        // arrange
        var expected = TestDataInitializer.GetTestIndexRecords();
        AddFundsAndCompanies(expected);

        MockedRepository
            .Setup(mock => 
                mock.GetAllAsync(
                    It.IsAny<Expression<Func<IndexRecord, bool>>?>(), 
                    It.IsAny<Expression<Func<IndexRecord, object>>[]>()))
            .Returns(Task.FromResult((IEnumerable<IndexRecord>)expected))
            .Verifiable();
        
        // act
        var indexRecordService = GetService();
        var actual = (await indexRecordService.FetchAllAsync()).ToList();
        
        // assert
        Assert.IsNotEmpty(actual);
        Assert.That(actual.Count, Is.EqualTo(expected.Count));
        Assert.That(actual, Is.EquivalentTo(expected));
        
        Assert.True(actual.TrueForAll(x => x.Company is not null));
        Assert.True(actual.TrueForAll(x => x.CompanyId.Equals(x.Company!.Id)));
        
        Assert.True(actual.TrueForAll(x => x.Fund is not null));
        Assert.True(actual.TrueForAll(x => x.FundId.Equals(x.Fund!.Id)));
        
        MockedRepository.Verify(repo => repo.GetAllAsync(
            It.IsAny<Expression<Func<IndexRecord, bool>>?>(), 
            It.IsAny<Expression<Func<IndexRecord, object>>[]>()));
    }
    
    [Test]
    public async Task FetchFilteredAsync_IndexRecordFilterApplied_ReturnsFilteredIndexRecords()
    {
        // arrange
        var dateFilter = new DateOnly(2024, 1, 1);

        var expectedItems = TestDataInitializer.GetTestIndexRecords()
            .Where(fund => fund.IssueDate.Equals(dateFilter))
            .ToList();
        AddFundsAndCompanies(expectedItems);
        
        IndexRecordFilter filter = new IndexRecordFilter { GE_IssueDate = dateFilter, LE_IssueDate = dateFilter};
        
        QueryParams queryParams = new QueryParams
        {
            PageNumber = 1,
            PageSize = 10,
            SortAscending = true,
            SortParameter = "IssueDate"
        };

        QueryResult<IndexRecord> expected = new QueryResult<IndexRecord>()
        {
            PageNumber = 1,
            PageSize = 10,
            Items = expectedItems,
            PagingEnabled = true,
            TotalItemsCount = expectedItems.Count
        };

        MockedQuery
            .Setup(mock => mock.CountTotalAsync())
            .Returns(Task.FromResult(expectedItems.Count))
            .Verifiable();

        MockedQuery
            .Setup(mock => mock.ExecuteAsync())
            .Returns(Task.FromResult(expected))
            .Verifiable();
        
        MockedQuery
            .Setup(mock => mock.Filter)
            .Returns(filter);

        MockedQuery
            .Setup(mock => mock.QueryParams)
            .Returns(queryParams);
        
        // act
        var indexRecordService = GetService();
        var actual = await indexRecordService.FetchFilteredAsync(filter, queryParams);
        
        // assert
        Assert.NotNull(actual);
        Assert.IsNotEmpty(actual.Items);
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(actual.Items, Is.EquivalentTo(expected.Items));

        var actualItemList = actual.Items.ToList();
        
        Assert.True(actualItemList.TrueForAll(x => x.Company is not null));
        Assert.True(actualItemList.TrueForAll(x => x.CompanyId.Equals(x.Company!.Id)));
        
        Assert.True(actualItemList.TrueForAll(x => x.Fund is not null));
        Assert.True(actualItemList.TrueForAll(x => x.FundId.Equals(x.Fund!.Id)));
        
        MockedQuery.Verify(query => query.Where(It.IsAny<Expression<Func<IndexRecord, bool>>?>()));
        MockedQuery.Verify(query => query.CountTotalAsync());
        MockedQuery.Verify(query => query.ExecuteAsync());
    }

    [Test]
    public async Task FindByIdAsync_IndexRecordNotNull_ReturnIndexRecord()
    {
        // arrange
        var expected = TestDataInitializer.GetTestIndexRecord();

        MockedRepository
            .Setup(mock => mock.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(expected)!)
            .Verifiable();
        
        // act
        var indexRecordService = GetService();
        var actual = await indexRecordService.FindByIdAsync(expected.Id);
        
        // assert
        Assert.NotNull(actual);
        Assert.That(actual, Is.EqualTo(expected));
        
        MockedRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<long>()));
    }

    [Test]
    public async Task FindByIdAsync_IndexRecordIsNull_ThrowsNoSuchEntityException()
    {
        // arrange
        var nonExistingId = 100000L;
        
        MockedRepository
            .Setup(mock => mock.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult<IndexRecord>(null!)!);
        
        // act
        var indexRecordService = GetService();
        Assert.ThrowsAsync<NoSuchEntityException<long>>(async () => await indexRecordService.FindByIdAsync(nonExistingId));
        
        MockedRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<long>()));
    }

    [Test]
    public async Task DeleteAsync_ExistingEntityToDelete_DeleteIsCalled()
    {
        // arrange
        var toDelete = TestDataInitializer.GetTestIndexRecord();
        
        MockedRepository
            .Setup(mock => mock.Delete(It.IsAny<IndexRecord>()))
            .Verifiable();
        
        // act
        var indexRecordService = GetService();
        await indexRecordService.DeleteAsync(toDelete);
            
        // assert
        MockedRepository.Verify(repo => repo.Delete(It.IsAny<IndexRecord>()));
        MockedUoW.Verify(uow => uow.CommitAsync());
    }
    
    [Test]
    public async Task DeleteRangeAsync_ExistingEntitiesToDelete_DeleteIsCalled()
    {
        // arrange
        var toDelete = TestDataInitializer.GetTestIndexRecords().ToArray();
        
        MockedRepository
            .Setup(mock => mock.DeleteRange(It.IsAny<IndexRecord[]>()))
            .Verifiable();
        
        // act
        var indexRecordService = GetService();
        await indexRecordService.DeleteRangeAsync(toDelete);
            
        // assert
        MockedRepository.Verify(repo => repo.DeleteRange(It.IsAny<IndexRecord[]>()));
        MockedUoW.Verify(uow => uow.CommitAsync());
    }

    [TestCase(10.12, 500.00)]
    [TestCase(-10.123, -0.00)]
    [TestCase(100, 0)]
    [TestCase(-1000, 1000)]
    public async Task FetchFilteredMinimalAsync_IndexRecordFilterAppliedNoQueryParams_ReturnsFilteredIndexRecords(double marketValueGreaterEqual, double marketValueLessEqual)
    {
        // arrange
        var expectedItems = TestDataInitializer.GetTestIndexRecords()
            .Where(fund => fund.MarketValue >= marketValueGreaterEqual && fund.MarketValue <= marketValueLessEqual)
            .ToList();

        IndexRecordFilter filter = new IndexRecordFilter
            { GE_MarketValue = marketValueGreaterEqual, LE_MarketValue = marketValueLessEqual };
        
        QueryResult<IndexRecord> expected = new QueryResult<IndexRecord>()
        {
            Items = expectedItems,
            PagingEnabled = false
        };

        MockedQuery
            .Setup(mock => mock.ExecuteAsync())
            .Returns(Task.FromResult(expected))
            .Verifiable();
        
        MockedQuery
            .Setup(mock => mock.Filter)
            .Returns(filter);
        
        // act
        var indexRecordService = GetService();
        var actual = await indexRecordService.FetchFilteredMinimalAsync(filter, null);
        
        // assert
        Assert.NotNull(actual);
        Assert.That(actual, Is.EqualTo(expected));
        Assert.NotNull(actual.Items);
        Assert.That(actual.Items.Count(), Is.EqualTo(expectedItems.Count()));
        Assert.That(actual.Items, Is.EquivalentTo(expectedItems));
        
        MockedQuery.Verify(query => query.Where(It.IsAny<Expression<Func<IndexRecord, bool>>?>()));
        MockedQuery.Verify(query => query.ExecuteAsync());
    }

    private static List<Tuple<string, DateOnly>> FetchByDateAndFundNameAsync() =>
    [
        new("STARK", new DateOnly(2024, 1, 1)),
        new("STARK", new DateOnly(2024, 1, 2)),
        new("DoesNotExist", new DateOnly(2030, 1, 1))
    ];

    [TestCaseSource(nameof(FetchByDateAndFundNameAsync))]
    public async Task FetchByDateAndFundNameAsync_FundNameAndDateFilters_ReturnsFilteredEntities(Tuple<string, DateOnly> testCase)
    {
        // arrange
        var fundName = testCase.Item1;
        var date = testCase.Item2;

        var expectedItems = TestDataInitializer.GetTestIndexRecords()
            .Where(x => x.IssueDate.Equals(date) 
                        && (x.FundId.Equals(TestDataInitializer.GetTestFunds().Find(y => y.FundName.Equals(fundName))?.Id)))
            .ToList();
        AddFundsAndCompanies(expectedItems);
        
        QueryResult<IndexRecord> expected = new QueryResult<IndexRecord>()
        {
            Items = expectedItems,
            PagingEnabled = false
        };
        
        MockedQuery
            .Setup(mock => mock.ExecuteAsync())
            .Returns(Task.FromResult(expected))
            .Verifiable();
        
        MockedQuery
            .Setup(mock => mock.Filter)
            .Returns(new IndexRecordFilter { CONTAINS_Fund_FundName = fundName, GE_IssueDate = date, LE_IssueDate = date });
        
        // act
        var indexRecordService = GetService();
        var actual = (await indexRecordService.FetchByDateAndFundNameAsync(fundName, date)).ToList();
        
        // assert
        Assert.NotNull(actual);
        Assert.That(actual.Count, Is.EqualTo(expectedItems.Count));
        Assert.True(actual.TrueForAll(x => x.Fund is not null && x.Fund.FundName.Equals(fundName)));
        Assert.True(actual.TrueForAll(x => x.IssueDate.Equals(date)));
        
        MockedQuery.Verify(mock => mock.ExecuteAsync());
    }

    [TestCaseSource(nameof(FetchByDateAndFundNameAsync))]
    public async Task ExistByDateAndFundNameAsync_FundNameAndDateFilters_ReturnsFoundOrNotFound(Tuple<string, DateOnly> testCase)
    {
        // arrange
        var fundName = testCase.Item1;
        var date = testCase.Item2;
        
        var expectedCount = TestDataInitializer
            .GetTestIndexRecords()
            .Count(x => x.IssueDate.Equals(date) 
                        && (x.FundId.Equals(TestDataInitializer.GetTestFunds().Find(y => y.FundName.Equals(fundName))?.Id)));

        MockedQuery
            .Setup(mock => mock.CountTotalAsync())
            .Returns(Task.FromResult(expectedCount))
            .Verifiable();
        
        // act
        var indexRecordService = GetService();
        var actual = await indexRecordService.ExistByDateAndFundNameAsync(fundName, date);
        
        // assert
        Assert.That(actual, Is.EqualTo(expectedCount is not 0));
        
        MockedQuery.Verify(mock => mock.Include(It.IsAny<Expression<Func<IndexRecord, object?>>[]>()));
        MockedQuery.Verify(mock => mock.Where(It.IsAny<Expression<Func<IndexRecord, bool>>?>()));
        MockedQuery.Verify(mock => mock.CountTotalAsync());
    }

    [Test]
    public async Task FetchComparableOlderDateAsync_FundNameAndDateFilters_ReturnsOlderDate()
    {
        // arrange
        var fundName = "STARK";
        var date = new DateOnly(2024, 1, 2);

        var testData = TestDataInitializer.GetTestIndexRecords()
            .Where(rec => rec.IssueDate < date)
            .OrderByDescending(rec => rec.IssueDate)
            .ToList();
        AddFundsAndCompanies(testData);
        testData = testData.Where(rec => rec.Fund!.FundName.Equals(fundName)).ToList();
        
        Assert.IsNotEmpty(testData);

        var expected = testData.First().IssueDate;
        
        var filter = new IndexRecordDateFilter { LT_IssueDate = date, EQ_Fund_FundName = fundName };
        var queryParams = new QueryParams
            { PageNumber = 1, PageSize = 1, SortAscending = false, SortParameter = "IssueDate" };
        
        MockedQuery
            .Setup(mock => mock.Filter)
            .Returns(filter);

        MockedQuery
            .Setup(mock => mock.QueryParams)
            .Returns(queryParams);

        MockedQuery
            .Setup(mock => mock.ExecuteAsync())
            .Returns(Task.FromResult(new QueryResult<IndexRecord> { Items = testData }));
        
        // act
        var indexRecordService = GetService();
        var actual = await indexRecordService.FetchComparableOlderDateAsync(fundName, date);
        
        // assert
        Assert.NotNull(actual);
        Assert.That(actual, Is.LessThan(date));
        Assert.That(actual, Is.EqualTo(expected));
        
        MockedQuery.Verify(mock => mock.ExecuteAsync());
    }
}
