using GenericInfrastructure.Query;
using Microsoft.Extensions.DependencyInjection;
using StockInfrastructure.UnitOfWork;
using TestUtilities;

namespace StockInfrastructureTests.Company;

public class CompanyQueryTests : InfrastructureBaseTest
{
    [TestCase("INC", "O")]
    [TestCase("OK", "")]
    [TestCase("DoesNotExist", "")]
    [TestCase("", "HOOD")]
    public async Task CountTotalAsync_NameAndTickerRestrictions_CountAllEntities(string nameRestriction, string tickerRestriction)
    {
        // arrange
        var expectedLength = TestDataInitializer
            .GetTestCompanies()
            .Count(x => x.CompanyName.Contains(nameRestriction) && x.Ticker.Contains(tickerRestriction));
        
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var query = uow.CompanyQuery;
        
        query.Reset();
        var actual = 
            await query
                .Where(x => x.CompanyName.Contains(nameRestriction) && x.Ticker.Contains(tickerRestriction))
                .CountTotalAsync();
        
        // assert
        Assert.That(actual, Is.EqualTo(expectedLength));
    }

    [TestCase("INC")]
    [TestCase("")]
    public async Task ExecuteAsync_NameRestriction_ReturnsFilteredEntities(string nameRestriction)
    {
        // arrange
        var expectedLength = TestDataInitializer
            .GetTestCompanies()
            .Count(x => x.CompanyName.Contains(nameRestriction));
        
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var query = uow.CompanyQuery;
        
        query.Reset();
        var actual = 
            await query
                .Where(x => x.CompanyName.Contains(nameRestriction))
                .ExecuteAsync();
        
        // assert
        Assert.NotNull(actual);
        Assert.That(actual.Items.Count(), Is.EqualTo(expectedLength));
        Assert.That(actual.PagingEnabled, Is.False);
    }

    [Test]
    public async Task ExecuteAsync_SortByAndPageRestrictions_ReturnsFilteredEntities()
    {
        // arrange
        var queryParams = new QueryParams
        {
            PageNumber = 1, 
            PageSize = 2,
            SortParameter = "CompanyName",
            SortAscending = true
        };
        
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var query = uow.CompanyQuery;
        
        query.Reset();
        query.QueryParams = queryParams;
        var actual =
            await query
                .SortBy(queryParams.SortParameter, queryParams.SortAscending)
                .Page(queryParams.PageNumber, queryParams.PageSize)
                .ExecuteAsync();
        
        // assert
        Assert.NotNull(actual);
        Assert.IsNotEmpty(actual.Items);
        Assert.That(actual.PagingEnabled, Is.True);
        Assert.That(actual.PageSize, Is.EqualTo(queryParams.PageSize));
        Assert.That(actual.PageNumber, Is.EqualTo(queryParams.PageNumber));
        Assert.That(actual.Items.Count(), Is.LessThanOrEqualTo(queryParams.PageSize));
        Assert.That(actual.Items.Select(x => x.CompanyName), Is.Ordered.Ascending);
    }

    [Test]
    public async Task ExecuteAsync_SortByAndPageAndWhereRestrictions_ReturnsFilteredEntities()
    {
        // arrange
        var queryParams = new QueryParams
        {
            PageNumber = 1, 
            PageSize = 2,
            SortParameter = "CompanyName",
            SortAscending = true
        };
        var nameFilter = "INC";
        
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var query = uow.CompanyQuery;
        
        query.Reset();
        query.QueryParams = queryParams;
        var actual =
            await query
                .Where(x => x.CompanyName.Contains(nameFilter))
                .SortBy(queryParams.SortParameter, queryParams.SortAscending)
                .Page(queryParams.PageNumber, queryParams.PageSize)
                .ExecuteAsync();
        
        Assert.IsNotEmpty(actual.Items);
        Assert.That(actual.PagingEnabled, Is.True);
        Assert.That(actual.PageSize, Is.EqualTo(queryParams.PageSize));
        Assert.That(actual.PageNumber, Is.EqualTo(queryParams.PageNumber));
        Assert.That(actual.Items.Count(), Is.LessThanOrEqualTo(queryParams.PageSize));
        Assert.That(actual.Items.Select(x => x.CompanyName), Is.Ordered.Ascending);
        actual.Items.ToList().ForEach(x => Assert.True(x.CompanyName.Contains(nameFilter)));
    }
}
