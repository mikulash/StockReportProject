using DataAccessLayer.Data;
using GenericInfrastructure.Query;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using TestUtilities;

namespace InfrastructureTests.Fund;

public class FundQueryTests
{
    private MockedDependencyInjectionBuilder _serviceProviderBuilder = null!;
    private IServiceScope _serviceScope = null!;

    [OneTimeSetUp]
    public void InitializeOnce()
    {
        _serviceProviderBuilder = new MockedDependencyInjectionBuilder()
            .AddDataAccessLayer(InfrastructureTestUtilities.CreateConfig())
            .AddInfrastructure();
    }

    [SetUp]
    public async Task Initialize()
    {
        _serviceScope = _serviceProviderBuilder.Create().CreateScope();

        var dbContext = _serviceScope.ServiceProvider.GetRequiredService<StockDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.Funds.AddRangeAsync(TestDataInitializer.GetTestFunds());
        await dbContext.SaveChangesAsync();
    }
    
    [TearDown]
    public void AfterEach()
    {
        _serviceScope.Dispose();
    }

    [Test]
    public async Task ExecuteAsync_NoRestrictions_ReturnsAllEntities()
    {
        // arrange
        var length = TestDataInitializer.GetTestFunds().Count;
        var queryParams = new QueryParams { PageNumber = 1, PageSize = 20, };
        
        // act
        using var uow = _serviceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var query = uow.FundQuery;
        
        query.Reset();
        query.QueryParams = queryParams;
        query.Filter = null;

        var actual = await query.ExecuteAsync();
        
        // assert
        Assert.NotNull(actual);
        Assert.IsNotEmpty(actual.Items);
        Assert.That(actual.Items.Count(), Is.EqualTo(length));
        Assert.That(actual.PagingEnabled, Is.True);
        Assert.That(actual.PageNumber, Is.EqualTo(queryParams.PageNumber));
        Assert.That(actual.PageSize, Is.EqualTo(queryParams.PageSize));
    }
    
    [TestCase("RK")]
    [TestCase("BEST")]
    [TestCase("DoesNotExist")]
    [TestCase("")]
    public async Task CountTotalAsync_NoRestrictions_CountAllEntities(string nameRestriction)
    {
        // arrange
        var expectedLength = TestDataInitializer
            .GetTestFunds()
            .Count(x => x.FundName.Contains(nameRestriction));
        
        // act
        using var uow = _serviceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var query = uow.FundQuery;
        
        query.Reset();
        var actual = 
            await query
                .Where(x => x.FundName.Contains(nameRestriction))
                .CountTotalAsync();
        
        // assert
        Assert.That(actual, Is.EqualTo(expectedLength));
    }

    [TestCase("ARK")]
    [TestCase("UTF")]
    [TestCase("")]
    public async Task ExecuteAsync_NameRestriction_ReturnsFilteredEntities(string nameRestriction)
    {
        // arrange
        var expectedLength = TestDataInitializer
            .GetTestFunds()
            .Count(x => x.FundName.Contains(nameRestriction));
        
        // act
        using var uow = _serviceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var query = uow.FundQuery;
        
        query.Reset();
        var actual = 
            await query
                .Where(x => x.FundName.Contains(nameRestriction))
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
            SortParameter = "FundName",
            SortAscending = true
        };
        
        // act
        using var uow = _serviceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var query = uow.FundQuery;
        
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
        Assert.That(actual.Items.Select(x => x.FundName), Is.Ordered.Ascending);
    }
}
