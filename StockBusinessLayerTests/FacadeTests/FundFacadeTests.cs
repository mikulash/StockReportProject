using DataAccessLayer.Models;
using GenericBusinessLayer.Exceptions;
using GenericBusinessLayer.Services;
using GenericInfrastructure.Query;
using GenericInfrastructure.Query.Filters;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StockAPI.DTOs.FundDTO.Create;
using StockAPI.DTOs.FundDTO.Filter;
using StockAPI.DTOs.FundDTO.Update;
using StockAPI.DTOs.FundDTO.View;
using TestUtilities;

using FundFacade = GenericBusinessLayer.Facades.IGenericFacade<DataAccessLayer.Models.Fund, long, 
    GenericBusinessLayer.Services.IGenericService<DataAccessLayer.Models.Fund, long>, 
    StockAPI.DTOs.FundDTO.Create.CreateFundDto, StockAPI.DTOs.FundDTO.Update.UpdateFundDto, 
    StockAPI.DTOs.FundDTO.View.ViewFundDto, StockAPI.DTOs.FundDTO.View.ViewFundDto>;

namespace StockBusinessLayerTests.FacadeTests;

public class FundFacadeTests
{
    private MockedDependencyInjectionBuilder _serviceProviderBuilder = null!;
    private Mock<IGenericService<Fund, long>> _mockedFundService = null!;

    [SetUp]
    public void Initialize()
    {
        _serviceProviderBuilder = new MockedDependencyInjectionBuilder()
            .AddInfrastructure()
            .AddBusinessLayer();

        _mockedFundService = new();
    }

    private ServiceProvider CreateServiceProvider() =>
        _serviceProviderBuilder
            .AddScoped(_mockedFundService.Object)
            .Create();
    
    private FundFacade GetFacade()
    {
        var serviceProvider = CreateServiceProvider();

        using var scope = serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<FundFacade>();
    }

    [Test]
    public async Task CreateAsync_NewEntity_ReturnsCreatedEntity()
    {
        // arrange
        var fund = TestDataInitializer.GetTestFund();
        var create = new CreateFundDto { FundName = fund.FundName };
        var expected = new ViewFundDto { Id = fund.Id, FundName = fund.FundName };
        
        _mockedFundService
            .Setup(mock => mock.CreateAsync(It.IsAny<Fund>(), It.IsAny<bool>()))
            .Returns(Task.FromResult(fund))
            .Verifiable();
        
        // act
        var facade = GetFacade();
        var actual = await facade.CreateAsync(create);
        
        // assert
        Assert.NotNull(actual);
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.FundName, Is.EqualTo(expected.FundName));
        
        _mockedFundService.Verify(service => service.CreateAsync(It.IsAny<Fund>(), It.IsAny<bool>()));
    }

    [Test]
    public async Task UpdateAsync_ValidEntityFound_ReturnsUpdatedEntity()
    {
        // arrange
        var fund = TestDataInitializer.GetTestFund();
        var update = new UpdateFundDto { FundName = "SomeOtherName" };
        var updated = new Fund { Id = fund.Id, FundName = update.FundName };
        var expected = new ViewFundDto { Id = updated.Id, FundName = updated.FundName };

        _mockedFundService
            .Setup(mock => mock.FindByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(fund))
            .Verifiable();

        _mockedFundService
            .Setup(mock => mock.UpdateAsync(It.IsAny<Fund>(), It.IsAny<bool>()))
            .Returns(Task.FromResult(updated))
            .Verifiable();
        
        // act
        var facade = GetFacade();
        var actual = await facade.UpdateAsync(fund.Id, update);
        
        // assert
        Assert.NotNull(actual);
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.FundName, Is.EqualTo(expected.FundName));
        
        _mockedFundService.Verify(mock => mock.FindByIdAsync(It.IsAny<long>()));
        _mockedFundService.Verify(mock => mock.UpdateAsync(It.IsAny<Fund>(), It.IsAny<bool>()));
    }

    [Test]
    public async Task FindByIdAsync_EntityFound_ReturnsFoundEntity()
    {
        // arrange
        var fund = TestDataInitializer.GetTestFund();
        var expected = new ViewFundDto { Id = fund.Id, FundName = fund.FundName };
        
        _mockedFundService
            .Setup(mock => mock.FindByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(fund))
            .Verifiable();
        
        // act
        var facade = GetFacade();
        var actual = await facade.FindByIdAsync(fund.Id);
        
        // assert
        Assert.NotNull(actual);
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.FundName, Is.EqualTo(expected.FundName));
        
        _mockedFundService.Verify(mock => mock.FindByIdAsync(It.IsAny<long>()));
    }

    [Test]
    public void FindByIdAsync_EntityNotFound_ThrowsNoSuchEntityException()
    {
        // arrange
        var nonExistingId = 100000L;

        _mockedFundService
            .Setup(mock => mock.FindByIdAsync(It.IsAny<long>()))
            .ThrowsAsync(new NoSuchEntityException<long>(typeof(Fund)));
        
        // act
        var facade = GetFacade();
        
        // assert
        Assert.ThrowsAsync<NoSuchEntityException<long>>(async () => await facade.FindByIdAsync(nonExistingId));
        
        _mockedFundService.Verify(mock => mock.FindByIdAsync(It.IsAny<long>()));
    }

    [Test]
    public async Task DeleteByIdAsync_EntityFound_DeleteCalled()
    {
        // arrange
        var fund = TestDataInitializer.GetTestFund();
        
        _mockedFundService
            .Setup(mock => mock.FindByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(fund))
            .Verifiable();
        
        _mockedFundService
            .Setup(mock => mock.DeleteAsync(It.IsAny<Fund>(), It.IsAny<bool>()))
            .Verifiable();
        
        // act
        var facade = GetFacade();
        await facade.DeleteByIdAsync(fund.Id);
        
        // assert
        _mockedFundService.Verify(mock => mock.FindByIdAsync(It.IsAny<long>()));
        _mockedFundService.Verify(mock => mock.DeleteAsync(It.IsAny<Fund>(), It.IsAny<bool>()));
    }

    [Test]
    public async Task FetchAllAsync_AllExistingEntities_ReturnsListOfEntities()
    {
        // arrange
        var funds = TestDataInitializer.GetTestFunds();
        
        _mockedFundService
            .Setup(mock => mock.FetchAllAsync())
            .Returns(Task.FromResult((IEnumerable<Fund>)funds))
            .Verifiable();
        
        // act
        var facade = GetFacade();
        var actual = (await facade.FetchAllAsync()).ToList();
        
        // assert
        Assert.NotNull(actual);
        Assert.IsNotEmpty(actual);
        Assert.That(actual.Count, Is.EqualTo(funds.Count));
        
        var item = actual.First(x => x.Id == 1);
        var entityItem = funds.First(x => x.Id == 1);
        Assert.That(item.FundName, Is.EqualTo(entityItem.FundName));
        
        _mockedFundService.Verify(mock => mock.FetchAllAsync());
    }

    [Test]
    public async Task FetchAllFilteredAsync_FilteredEntities_ReturnsListOfFilteredEntities()
    {
        // arrange
        var funds = TestDataInitializer.GetTestFunds().Where(fund => fund.FundName.Contains("ARK"));
        var filter = new FundFilterDto
        {
            CONTAINS_FundName = "ARK",
            PageSize = 10,
            PageNumber = 1
        };

        var enumerable = funds.ToList();
        var expected = new QueryResult<Fund>
        {
            PageNumber = 1,
            PageSize = 10,
            Items = enumerable,
            PagingEnabled = true,
            TotalItemsCount = enumerable.Count()
        };

        _mockedFundService
            .Setup(mock => mock.FetchFilteredAsync(It.IsAny<IFilter<Fund>>(), It.IsAny<QueryParams?>()))
            .Returns(Task.FromResult(expected))
            .Verifiable();
        
        // act
        var facade = GetFacade();
        var actual = await facade.FetchAllFilteredAsync(filter);
        
        // assert
        Assert.NotNull(actual);
        Assert.IsNotEmpty(actual.Items);
        Assert.That(actual.Items.Count(), Is.EqualTo(expected.Items.Count()));
        
        var item = actual.Items.First(x => x.Id == 2);
        var entityItem = expected.Items.First(x => x.Id == 2);
        Assert.That(item.FundName, Is.EqualTo(entityItem.FundName));
        
        _mockedFundService.Verify(mock => mock.FetchFilteredAsync(It.IsAny<IFilter<Fund>>(), It.IsAny<QueryParams?>()));
    }
    
}
