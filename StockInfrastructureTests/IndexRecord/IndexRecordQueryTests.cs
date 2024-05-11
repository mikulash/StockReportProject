using GenericInfrastructure.Query;
using Microsoft.Extensions.DependencyInjection;
using StockInfrastructure.UnitOfWork;
using TestUtilities;

using ExecuteAsyncTuple = System.Tuple<GenericInfrastructure.Query.QueryParams?,
    System.Linq.Expressions.Expression<System.Func<DataAccessLayer.Models.IndexRecord, bool>>?>;

namespace StockInfrastructureTests.IndexRecord;

public class IndexRecordQueryTests : InfrastructureBaseTest
{
    public static IEnumerable<Tuple<DateOnly, double>> CountTotalAsyncTestCases() =>
    [
        new(new DateOnly(2024, 1, 1), 120),
        new(new DateOnly(2000, 1, 1), 50),
        new(new DateOnly(2024, 1, 2), 50000)
    ];
    
    [TestCaseSource(nameof(CountTotalAsyncTestCases))]
    public async Task CountTotalAsync_NameAndTickerRestrictions_CountAllEntities(Tuple<DateOnly, double> testCase)
    {
        // arrange
        var expectedLength = TestDataInitializer
            .GetTestIndexRecords()
            .Count(x => x.IssueDate.Equals(testCase.Item1) && x.MarketValue < testCase.Item2);
        
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var query = uow.IndexRecordQuery;
        
        query.Reset();
        var actual = 
            await query
                .Where(x => x.IssueDate.Equals(testCase.Item1) && x.MarketValue < testCase.Item2)
                .CountTotalAsync();
        
        // assert
        Assert.That(actual, Is.EqualTo(expectedLength));
    }

    public static IEnumerable<ExecuteAsyncTuple> ExecuteAsyncTestCases() =>
    [
        new ExecuteAsyncTuple(
            null,
            rec => rec.CompanyId <= 4
        ),
        new ExecuteAsyncTuple(
            new QueryParams { PageNumber = 1, PageSize = 3 },
            null),
        new ExecuteAsyncTuple(
            new QueryParams { PageNumber = 1, PageSize = 3, SortParameter = "IssueDate", SortAscending = true },
            null),
        new ExecuteAsyncTuple(
            new QueryParams { PageNumber = 1, PageSize = 3, SortParameter = "IssueDate", SortAscending = false },
            x => x.IssueDate.Equals(new DateOnly(2024, 1, 2))),
        new ExecuteAsyncTuple(
            new QueryParams { PageNumber = 2, PageSize = 4 },
            x => x.Shares >= 100 && x.Shares <= 5000),
        new ExecuteAsyncTuple(
            new QueryParams { PageNumber = 2, PageSize = 20},
            null),
        new ExecuteAsyncTuple(
            new QueryParams {PageNumber = 1, PageSize = 1, SortAscending = true, SortParameter = "Weight"},
            x => x.Weight > 0),
        new ExecuteAsyncTuple(
            new QueryParams { PageNumber = 1, PageSize = 10},
            x => x.IssueDate >= new DateOnly(2024, 1, 1) && x.IssueDate <= new DateOnly(2024, 12, 24))
    ];

    [TestCaseSource(nameof(ExecuteAsyncTestCases))]
    public async Task ExecuteAsync_MultipleRestrictionByTestCase_ReturnsFilteredEntities(ExecuteAsyncTuple testCase)
    {
        // arrange
        var queryParams = testCase.Item1;
        var condition = testCase.Item2;
        var expectedCount = TestDataInitializer.GetTestIndexRecords()
            .Where(condition?.Compile() ?? (_ => true))
            .Skip(((queryParams?.PageNumber ?? 0) - 1) * (queryParams?.PageSize ?? 0))
            .Take(queryParams?.PageSize ?? TestDataInitializer.GetTestIndexRecords().Count)
            .Count();
            

        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var query = uow.IndexRecordQuery;
        
        query.Reset();

        if (condition is not null)
        {
            query.Where(condition);
        }
        
        if (queryParams is not null)
        {
            query.QueryParams = queryParams;
            query.SortBy(queryParams.SortParameter, queryParams.SortAscending);
            query.Page(queryParams.PageNumber, queryParams.PageSize);
        }
        query.Include(x => x.Fund, x => x.Company);

        var actual = await query.ExecuteAsync();

        // assert
        Assert.NotNull(actual);
        Assert.That(actual.PagingEnabled, Is.EqualTo(queryParams is not null));
        if (queryParams is not null)
        {
            Assert.That(actual.PageSize, Is.EqualTo(queryParams.PageSize));
            Assert.That(actual.PageNumber, Is.EqualTo(queryParams.PageNumber));
            Assert.That(actual.Items.Count(), Is.LessThanOrEqualTo(queryParams.PageSize));
            if (!queryParams.SortParameter.Equals(string.Empty))
            {
                Assert.That(actual.Items.Select(x => x.GetType().GetProperty(queryParams.SortParameter).GetValue(x)), 
                    queryParams.SortAscending ? Is.Ordered.Ascending : Is.Ordered.Descending);
            }
        }
        Assert.NotNull(actual.Items);
        Assert.That(actual.Items.Count(), Is.EqualTo(expectedCount));
    }
}
