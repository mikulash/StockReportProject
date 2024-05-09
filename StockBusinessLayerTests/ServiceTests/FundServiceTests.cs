using System.Linq.Expressions;
using DataAccessLayer.Models;
using GenericBusinessLayer.Exceptions;
using GenericBusinessLayer.Services;
using GenericDataAccessLayer.Models;
using GenericInfrastructure.Query;
using GenericInfrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StockInfrastructure.Query.Filters.EntityFilters;
using StockInfrastructure.UnitOfWork;
using TestUtilities;

namespace StockBusinessLayerTests.ServiceTests;

public class FundServiceTests : ServiceTestBase<Fund>
{
    [Test]
    public async Task CreateAsync_NewFundCorrectFormat_ReturnsCreatedFund()
    {
        // arrange
        var expected = TestDataInitializer.GetTestFund();
        
        MockedRepository
            .Setup(mock => mock.AddAsync(It.IsAny<Fund>()))
            .Returns(Task.FromResult(expected))
            .Verifiable();
        
        // act
        var fundService = GetService();
        var actual = await fundService.CreateAsync(expected);
            
        // assert
        Assert.NotNull(actual);
        Assert.That(actual, Is.EqualTo(expected));
        
        MockedRepository.Verify(repo => repo.AddAsync(It.IsAny<Fund>()));
        MockedUoW.Verify(uow => uow.CommitAsync());
    }

    [Test]
    public async Task CreateRangeAsync_NewFundsInCorrectFormat_ReturnsCreatedFunds()
    {
        // arrange
        var expected = TestDataInitializer.GetTestFunds().ToArray();

        MockedRepository
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
        
        MockedRepository.Verify(repo => repo.AddRangeAsync(It.IsAny<Fund[]>()));
        MockedUoW.Verify(uow => uow.CommitAsync());
    }

    [TestCase("NewFund")]
    [TestCase("")]
    public async Task UpdateAsync_NewFundName_ReturnsUpdatedFund(string newFundName)
    {
        // arrange
        var expected = TestDataInitializer.GetTestFund();
        expected.FundName = newFundName;
        
        MockedRepository
            .Setup(mock => mock.Update(It.IsAny<Fund>()))
            .Verifiable();
        
        // act
        var fundService = GetService();
        var actual = await fundService.UpdateAsync(expected);
            
        // assert
        Assert.NotNull(actual);
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(actual.FundName, Is.EqualTo(newFundName));
        
        MockedRepository.Verify(repo => repo.Update(It.IsAny<Fund>()));
        MockedUoW.Verify(uow => uow.CommitAsync());
    }

    [Test]
    public async Task FetchAllAsync_NoRestrictions_ReturnsAllFunds()
    {
        // arrange
        var expected = TestDataInitializer.GetTestFunds();

        MockedRepository
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
        
        MockedRepository.Verify(repo => repo.GetAllAsync(
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
        var fundService = GetService();
        var actual = await fundService.FetchFilteredAsync(filter, queryParams);
        
        // assert
        Assert.NotNull(actual);
        Assert.IsNotEmpty(actual.Items);
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(actual.Items, Is.EquivalentTo(expected.Items));
        
        MockedQuery.Verify(query => query.Where(It.IsAny<Expression<Func<Fund, bool>>?>()));
        MockedQuery.Verify(query => query.CountTotalAsync());
        MockedQuery.Verify(query => query.ExecuteAsync());
    }

    [Test]
    public async Task FindByIdAsync_FundNotNull_ReturnFund()
    {
        // arrange
        var expected = TestDataInitializer.GetTestFund();

        MockedRepository
            .Setup(mock => mock.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(expected)!)
            .Verifiable();
        
        // act
        var fundService = GetService();
        var actual = await fundService.FindByIdAsync(expected.Id);
        
        // assert
        Assert.NotNull(actual);
        Assert.That(actual, Is.EqualTo(expected));
        
        MockedRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<long>()));
    }

    [Test]
    public async Task FindByIdAsync_FundIsNull_ThrowsNoSuchEntityException()
    {
        // arrange
        var nonExistingId = 100000L;
        
        MockedRepository
            .Setup(mock => mock.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult<Fund>(null!)!);
        
        // act
        var fundService = GetService();
        Assert.ThrowsAsync<NoSuchEntityException<long>>(async () => await fundService.FindByIdAsync(nonExistingId));
        
        MockedRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<long>()));
    }

    [Test]
    public async Task DeleteAsync_ExistingEntityToDelete_DeleteIsCalled()
    {
        // arrange
        var toDelete = TestDataInitializer.GetTestFund();
        
        MockedRepository
            .Setup(mock => mock.Delete(It.IsAny<Fund>()))
            .Verifiable();
        
        // act
        var fundService = GetService();
        await fundService.DeleteAsync(toDelete);
            
        // assert
        MockedRepository.Verify(repo => repo.Delete(It.IsAny<Fund>()));
        MockedUoW.Verify(uow => uow.CommitAsync());
    }
    
    [Test]
    public async Task DeleteRangeAsync_ExistingEntitiesToDelete_DeleteIsCalled()
    {
        // arrange
        var toDelete = TestDataInitializer.GetTestFunds().ToArray();
        
        MockedRepository
            .Setup(mock => mock.DeleteRange(It.IsAny<Fund[]>()))
            .Verifiable();
        
        // act
        var fundService = GetService();
        await fundService.DeleteRangeAsync(toDelete);
            
        // assert
        MockedRepository.Verify(repo => repo.DeleteRange(It.IsAny<Fund[]>()));
        MockedUoW.Verify(uow => uow.CommitAsync());
    }
}
