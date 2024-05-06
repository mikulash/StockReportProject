
using System.Linq.Expressions;
using DataAccessLayer.Models;
using GenericBusinessLayer.Exceptions;
using GenericBusinessLayer.Services;
using GenericInfrastructure.Query;
using GenericInfrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StockInfrastructure.Query.Filters.EntityFilters;
using StockInfrastructure.UnitOfWork;
using TestUtilities;

namespace BusinessLayerTests.ServiceTests;

public class FundServiceTests
{
    private MockedDependencyInjectionBuilder _serviceProviderBuilder = null!;

    private Mock<IGenericRepository<Fund, long>> _mockedRepository = null!;
    private Mock<IQuery<Fund, long>> _mockedQuery = null!;

    private Mock<IStockUnitOfWork> _mockedUoW = null!;

    private void InitializeUoW()
    {
        _mockedUoW
            .Setup(mock => mock.GetRepositoryByEntity<Fund, long>())
            .Returns(_mockedRepository.Object);
        _mockedUoW
            .Setup(mock => mock.GetQueryByEntity<Fund, long>())
            .Returns(_mockedQuery.Object);
        _mockedUoW
            .Setup(mock => mock.CommitAsync())
            .Verifiable();
    }

    private void InitializeQuery()
    {
        _mockedQuery
            .Setup(mock => mock.Reset())
            .Verifiable();

        _mockedQuery
            .Setup(mock => mock.Where(It.IsAny<Expression<Func<Fund, bool>>?>()))
            .Returns(_mockedQuery.Object)
            .Verifiable();

        _mockedQuery
            .Setup(mock => mock.Include(It.IsAny<Expression<Func<Fund, object?>>[]>()))
            .Returns(_mockedQuery.Object)
            .Verifiable();
        
        _mockedQuery
            .Setup(mock => mock.Page(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(_mockedQuery.Object)
            .Verifiable();
        
        _mockedQuery
            .Setup(mock => mock.SortBy(It.IsAny<string>(), It.IsAny<bool>()))
            .Returns(_mockedQuery.Object)
            .Verifiable();
    }

    [SetUp]
    public void Initialize()
    {
       _serviceProviderBuilder = new MockedDependencyInjectionBuilder()
           .AddInfrastructure()
           .AddBusinessLayer();

       _mockedRepository = new();
       _mockedQuery = new();
       _mockedUoW = new();
       
       InitializeQuery();
       InitializeUoW();
    }

    private ServiceProvider CreateServiceProvider() =>
        _serviceProviderBuilder
            .AddScoped(_mockedRepository.Object)
            .AddScoped(_mockedUoW.Object)
            .Create();

    private IGenericService<Fund, long> GetService()
    {
        var serviceProvider = CreateServiceProvider();

        using var scope = serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IGenericService<Fund, long>>();
    }

    [Test]
    public async Task CreateAsync_NewFundCorrectFormat_ReturnsCreatedFund()
    {
        // arrange
        var expected = TestDataInitializer.GetTestFund();
        
        _mockedRepository
            .Setup(mock => mock.AddAsync(It.IsAny<Fund>()))
            .Returns(Task.FromResult(expected))
            .Verifiable();
        
        // act
        var fundService = GetService();
        var actual = await fundService.CreateAsync(expected);
            
        // assert
        Assert.NotNull(actual);
        Assert.That(actual, Is.EqualTo(expected));
        
        _mockedRepository.Verify(repo => repo.AddAsync(It.IsAny<Fund>()));
        _mockedUoW.Verify(uow => uow.CommitAsync());
    }

    [Test]
    public async Task CreateRangeAsync_NewFundsInCorrectFormat_ReturnsCreatedFunds()
    {
        // arrange
        var expected = TestDataInitializer.GetTestFunds().ToArray();

        _mockedRepository
            .Setup(mock => mock.AddRangeAsync(It.IsAny<Fund[]>()))
            .Returns(Task.FromResult(expected))
            .Verifiable();
        
        // act
        var fundService = GetService();
        var actual = (await fundService.CreateRangeAsync(expected)).ToArray();
            
        // assert
        Assert.NotNull(actual);
        Assert.That(actual.Length, Is.EqualTo(expected.Length));
        Assert.That(actual, Is.EquivalentTo(expected));
        
        _mockedRepository.Verify(repo => repo.AddRangeAsync(It.IsAny<Fund[]>()));
        _mockedUoW.Verify(uow => uow.CommitAsync());
    }

    [TestCase("NewFund")]
    [TestCase("")]
    public async Task UpdateAsync_NewFundName_ReturnsUpdatedFund(string newFundName)
    {
        // arrange
        var expected = TestDataInitializer.GetTestFund();
        expected.FundName = newFundName;
        
        _mockedRepository
            .Setup(mock => mock.Update(It.IsAny<Fund>()))
            .Verifiable();
        
        // act
        var fundService = GetService();
        var actual = await fundService.UpdateAsync(expected);
            
        // assert
        Assert.NotNull(actual);
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(actual.FundName, Is.EqualTo(newFundName));
        
        _mockedRepository.Verify(repo => repo.Update(It.IsAny<Fund>()));
        _mockedUoW.Verify(uow => uow.CommitAsync());
    }

    [Test]
    public async Task FetchAllAsync_NoRestrictions_ReturnsAllFunds()
    {
        // arrange
        var expected = TestDataInitializer.GetTestFunds();

        _mockedRepository
            .Setup(mock => 
                mock.GetAllAsync(
                    It.IsAny<Expression<Func<Fund, bool>>?>(), 
                    It.IsAny<Expression<Func<Fund, object>>[]>()))
            .Returns(Task.FromResult((IEnumerable<Fund>)expected))
            .Verifiable();
        
        // act
        var fundService = GetService();
        var actual = (await fundService.FetchAllAsync()).ToList();
        
        // assert
        Assert.IsNotEmpty(actual);
        Assert.That(actual.Count, Is.EqualTo(expected.Count));
        Assert.That(actual, Is.EquivalentTo(expected));
        
        _mockedRepository.Verify(repo => repo.GetAllAsync(
            It.IsAny<Expression<Func<Fund, bool>>?>(), 
            It.IsAny<Expression<Func<Fund, object>>[]>()));
    }
    
    [Test]
    public async Task FetchFilteredAsync_FundFilterApplied_ReturnsFilteredFunds()
    {
        // arrange
        string fundNameFilter = "ARK";

        var expectedItems = TestDataInitializer.GetTestFunds()
            .Where(fund => fund.FundName.Contains(fundNameFilter))
            .ToList();
        FundFilter filter = new FundFilter { CONTAINS_FundName = fundNameFilter };
        
        QueryParams queryParams = new QueryParams
        {
            PageNumber = 1,
            PageSize = 10,
            SortAscending = true,
            SortParameter = "FundName"
        };

        QueryResult<Fund> expected = new QueryResult<Fund>()
        {
            PageNumber = 1,
            PageSize = 10,
            Items = expectedItems,
            PagingEnabled = true,
            TotalItemsCount = expectedItems.Count
        };

        _mockedQuery
            .Setup(mock => mock.CountTotalAsync())
            .Returns(Task.FromResult(expectedItems.Count))
            .Verifiable();

        _mockedQuery
            .Setup(mock => mock.ExecuteAsync())
            .Returns(Task.FromResult(expected))
            .Verifiable();
        
        _mockedQuery
            .Setup(mock => mock.Filter)
            .Returns(filter);

        _mockedQuery
            .Setup(mock => mock.QueryParams)
            .Returns(queryParams);
        
        // act
        var fundService = GetService();
        var actual = await fundService.FetchFilteredAsync(filter, queryParams);
        
        // assert
        Assert.NotNull(actual);
        Assert.IsNotEmpty(actual.Items);
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(actual.Items, Is.EquivalentTo(expected.Items));
        
        _mockedQuery.Verify(query => query.Where(It.IsAny<Expression<Func<Fund, bool>>?>()));
        _mockedQuery.Verify(query => query.CountTotalAsync());
        _mockedQuery.Verify(query => query.ExecuteAsync());
    }

    [Test]
    public async Task FindByIdAsync_FundNotNull_ReturnFund()
    {
        // arrange
        var expected = TestDataInitializer.GetTestFund();

        _mockedRepository
            .Setup(mock => mock.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(expected)!)
            .Verifiable();
        
        // act
        var fundService = GetService();
        var actual = await fundService.FindByIdAsync(expected.Id);
        
        // assert
        Assert.NotNull(actual);
        Assert.That(actual, Is.EqualTo(expected));
        
        _mockedRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<long>()));
    }

    [Test]
    public async Task FindByIdAsync_FundIsNull_ThrowsNoSuchEntityException()
    {
        // arrange
        var nonExistingId = 100000L;
        
        _mockedRepository
            .Setup(mock => mock.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult<Fund>(null!)!);
        
        // act
        var fundService = GetService();
        Assert.ThrowsAsync<NoSuchEntityException<long>>(async () => await fundService.FindByIdAsync(nonExistingId));
        
        _mockedRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<long>()));
    }

    [Test]
    public async Task DeleteAsync_ExistingEntityToDelete_DeleteIsCalled()
    {
        // arrange
        var toDelete = TestDataInitializer.GetTestFund();
        
        _mockedRepository
            .Setup(mock => mock.Delete(It.IsAny<Fund>()))
            .Verifiable();
        
        // act
        var fundService = GetService();
        await fundService.DeleteAsync(toDelete);
            
        // assert
        _mockedRepository.Verify(repo => repo.Delete(It.IsAny<Fund>()));
        _mockedUoW.Verify(uow => uow.CommitAsync());
    }
    
    [Test]
    public async Task DeleteRangeAsync_ExistingEntitiesToDelete_DeleteIsCalled()
    {
        // arrange
        var toDelete = TestDataInitializer.GetTestFunds().ToArray();
        
        _mockedRepository
            .Setup(mock => mock.DeleteRange(It.IsAny<Fund[]>()))
            .Verifiable();
        
        // act
        var fundService = GetService();
        await fundService.DeleteRangeAsync(toDelete);
            
        // assert
        _mockedRepository.Verify(repo => repo.DeleteRange(It.IsAny<Fund[]>()));
        _mockedUoW.Verify(uow => uow.CommitAsync());
    }
}
