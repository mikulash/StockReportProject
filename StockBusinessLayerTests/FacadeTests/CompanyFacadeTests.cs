using DataAccessLayer.Models;
using GenericBusinessLayer.DTOs.BaseFilter;
using GenericBusinessLayer.Exceptions;
using GenericInfrastructure.Query;
using GenericInfrastructure.Query.Filters;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StockAPI.DTOs.CompanyDTOs.Create;
using StockAPI.DTOs.CompanyDTOs.Filter;
using StockAPI.DTOs.CompanyDTOs.Update;
using StockAPI.DTOs.CompanyDTOs.View;
using StockBusinessLayer.Facades.CompanyFacade;
using StockBusinessLayer.Services.CompanyService;
using StockBusinessLayer.Services.IndexRecordService;
using TestUtilities;

namespace StockBusinessLayerTests.FacadeTests;

public class CompanyFacadeTests : FacadeTestBase<ICompanyService>
{
    protected Mock<IIndexRecordService> MockedIndexRecordService = null!;

    [SetUp]
    public override void Initialize()
    {
        base.Initialize();
        MockedIndexRecordService = new Mock<IIndexRecordService>();
    }

    protected override ServiceProvider CreateServiceProvider()
    {
        ServiceProviderBuilder
            .AddScoped(MockedIndexRecordService.Object);
        return base.CreateServiceProvider();
    }

    private static void AssertEqualEntities(DetailedViewCompanyDto actual, DetailedViewCompanyDto expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.CompanyName, Is.EqualTo(expected.CompanyName));
        Assert.That(actual.Ticker, Is.EqualTo(expected.Ticker));
        Assert.That(actual.CUSIP, Is.EqualTo(expected.CUSIP));
    }
    
    private static void AssertEqualEntities(BasicViewCompanyDto actual, BasicViewCompanyDto expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.CompanyName, Is.EqualTo(expected.CompanyName));
        Assert.That(actual.Ticker, Is.EqualTo(expected.Ticker));
        Assert.That(actual.CUSIP, Is.EqualTo(expected.CUSIP));
    }
    
    [Test]
    public async Task CreateAsync_NewEntityProvided_ReturnsCreatedEntityAsDto()
    {
        // arrange
        var company = TestDataInitializer.GetTestCompany();
        var create = new CreateCompanyDto { CompanyName = company.CompanyName, CUSIP = company.CUSIP, Ticker = company.Ticker };
        var expected = new DetailedViewCompanyDto { Id = company.Id, CompanyName = company.CompanyName, CUSIP = company.CUSIP, Ticker = company.Ticker };
        
        MockedService
            .Setup(mock => mock.CreateAsync(It.IsAny<Company>(), It.IsAny<bool>()))
            .Returns(Task.FromResult(company))
            .Verifiable();
        
        // act
        var facade = GetFacade<ICompanyFacade>();
        var actual = await facade.CreateAsync(create);
        
        // assert
        Assert.NotNull(actual);
        AssertEqualEntities(actual, expected);
        
        MockedService.Verify(service => service.CreateAsync(It.IsAny<Company>(), It.IsAny<bool>()));
    }

    [Test]
    public async Task UpdateAsync_ValidEntityFound_ReturnsUpdatedEntity()
    {
        // arrange
        var company = TestDataInitializer.GetTestCompany();
        var update = new UpdateCompanyDto { CompanyName = "NewName", CUSIP = company.CUSIP, Ticker = company.Ticker };
        var updated = new Company
            { Id = company.Id, CompanyName = update.CompanyName, CUSIP = update.CUSIP, Ticker = update.Ticker };
        var expected = new DetailedViewCompanyDto() 
            { Id = updated.Id, CompanyName = update.CompanyName, CUSIP = update.CUSIP, Ticker = update.Ticker };

        MockedService
            .Setup(mock => mock.FindByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(company))
            .Verifiable();

        MockedService
            .Setup(mock => mock.UpdateAsync(It.IsAny<Company>(), It.IsAny<bool>()))
            .Returns(Task.FromResult(updated))
            .Verifiable();
        
        // act
        var facade = GetFacade<ICompanyFacade>();
        var actual = await facade.UpdateAsync(company.Id, update);
        
        // assert
        Assert.NotNull(actual);
        AssertEqualEntities(actual, expected);
        
        MockedService.Verify(mock => mock.FindByIdAsync(It.IsAny<long>()));
        MockedService.Verify(mock => mock.UpdateAsync(It.IsAny<Company>(), It.IsAny<bool>()));
    }

    [Test]
    public async Task FindByIdAsync_EntityFound_ReturnsFoundEntity()
    {
        // arrange
        var company = TestDataInitializer.GetTestCompany();
        var expected = new DetailedViewCompanyDto() 
            { Id = company.Id, CompanyName = company.CompanyName, CUSIP = company.CUSIP, Ticker = company.Ticker };
        
        MockedService
            .Setup(mock => mock.FindByIdAllIndexRecordsAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(company))
            .Verifiable();
        
        // act
        var facade = GetFacade<ICompanyFacade>();
        var actual = await facade.FindByIdAsync(company.Id);
        
        // assert
        Assert.NotNull(actual);
        AssertEqualEntities(actual, expected);
        
        MockedService.Verify(mock => mock.FindByIdAllIndexRecordsAsync(It.IsAny<long>()));
    }

    [Test]
    public void FindByIdAsync_EntityNotFound_ThrowsNoSuchEntityException()
    {
        // arrange
        var nonExistingId = 100000L;

        MockedService
            .Setup(mock => mock.FindByIdAllIndexRecordsAsync(It.IsAny<long>()))
            .ThrowsAsync(new NoSuchEntityException<long>(typeof(Company)));
        
        // act
        var facade = GetFacade<ICompanyFacade>();
        
        // assert
        Assert.ThrowsAsync<NoSuchEntityException<long>>(async () => await facade.FindByIdAsync(nonExistingId));
        
        MockedService.Verify(mock => mock.FindByIdAllIndexRecordsAsync(It.IsAny<long>()));
    }

    [Test]
    public async Task DeleteByIdAsync_EntityFound_DeleteCalled()
    {
        // arrange
        var company = TestDataInitializer.GetTestCompany();
        
        MockedService
            .Setup(mock => mock.FindByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(company))
            .Verifiable();
        
        MockedService
            .Setup(mock => mock.DeleteAsync(It.IsAny<Company>(), It.IsAny<bool>()))
            .Verifiable();
        
        // act
        var facade = GetFacade<ICompanyFacade>();
        await facade.DeleteByIdAsync(company.Id);
        
        // assert
        MockedService.Verify(mock => mock.FindByIdAsync(It.IsAny<long>()));
        MockedService.Verify(mock => mock.DeleteAsync(It.IsAny<Company>(), It.IsAny<bool>()));
    }

    [Test]
    public async Task FetchAllAsync_AllExistingEntities_ReturnsListOfEntities()
    {
        // arrange
        var companies = TestDataInitializer.GetTestCompanies();
        
        MockedService
            .Setup(mock => mock.FetchAllAsync())
            .Returns(Task.FromResult((IEnumerable<Company>)companies))
            .Verifiable();
        
        // act
        var facade = GetFacade<ICompanyFacade>();
        var actual = (await facade.FetchAllAsync()).ToList();
        
        // assert
        Assert.NotNull(actual);
        Assert.IsNotEmpty(actual);
        Assert.That(actual.Count, Is.EqualTo(companies.Count));
        for (int index = 0; index < actual.Count(); index++)
        {
            var company = companies.ElementAt(index);
            var expectedItem = new BasicViewCompanyDto
                { Id = company.Id, CompanyName = company.CompanyName, CUSIP = company.CUSIP, Ticker = company.Ticker };
            AssertEqualEntities(actual.ElementAt(index), expectedItem);
        }

        MockedService.Verify(mock => mock.FetchAllAsync());
    }

    [Test]
    public async Task FetchAllFilteredAsync_FilteredEntities_ReturnsListOfFilteredEntities()
    {
        // arrange
        var companies = TestDataInitializer.GetTestCompanies()
            .Where(comp => comp.CompanyName.Contains("INC"))
            .ToList();
        
        var filter = new CompanyFilterDto()
        {
            CONTAINS_CompanyName = "INC",
            PageSize = 10,
            PageNumber = 1
        };
        
        var expected = new QueryResult<Company>
        {
            PageNumber = 1,
            PageSize = 10,
            Items = companies,
            PagingEnabled = true,
            TotalItemsCount = companies.Count()
        };

        MockedService
            .Setup(mock => mock.FetchFilteredAsync(It.IsAny<IFilter<Company>>(), 
                It.IsAny<QueryParams?>()))
            .Returns(Task.FromResult(expected))
            .Verifiable();
        
        // act
        var facade = GetFacade<ICompanyFacade>();
        var actual = await facade.FetchAllFilteredAsync(filter);
        
        // assert
        Assert.NotNull(actual);
        Assert.IsNotEmpty(actual.Items);
        Assert.That(actual.Items.Count(), Is.EqualTo(expected.Items.Count()));

        for (int index = 0; index < actual.Items.Count(); index++)
        {
            var company = companies.ElementAt(index);
            var expectedItem = new BasicViewCompanyDto
                { Id = company.Id, CompanyName = company.CompanyName, CUSIP = company.CUSIP, Ticker = company.Ticker };
            AssertEqualEntities(actual.Items.ElementAt(index), expectedItem);
        }
        
        MockedService.Verify(mock => mock.FetchFilteredAsync(
            It.IsAny<IFilter<Company>>(), It.IsAny<QueryParams?>()));
    }

    [Test]
    public async Task FindByIdFilteredIndexRecords_ExistingCompanyWithPaging_ReturnsCompanyWithIndexRecords()
    {
        // arrange
        var company = TestDataInitializer.GetTestCompany();
        var items = TestDataInitializer.GetTestIndexRecords()
            .Where(rec => rec.CompanyId.Equals(company.Id))
            .ToList();
        var filter = new FilterDto { PageNumber = 1, PageSize = 20, SortAscending = true, SortParameter = "IssueDate" };

        var returnValue = new QueryResult<IndexRecord>
        {
            PageNumber = filter.PageNumber.Value,
            PageSize = filter.PageSize.Value,
            PagingEnabled = true,
            TotalItemsCount = items.Count,
            Items = items
        };
        
        MockedService
            .Setup(mock => mock.FindByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(company))
            .Verifiable();
        
        MockedIndexRecordService
            .Setup(mock => mock.FetchFilteredMinimalAsync(It.IsAny<IFilter<IndexRecord>>(), 
                It.IsAny<QueryParams>()))
            .Returns(Task.FromResult(returnValue))
            .Verifiable();
        
        // act
        var facade = GetFacade<ICompanyFacade>();
        var actual = await facade.FindByIdFilteredIndexRecords(company.Id, filter);
        
        // assert
        Assert.NotNull(actual);
        
        Assert.That(actual.Id, Is.EqualTo(company.Id));
        Assert.That(actual.CUSIP, Is.EqualTo(company.CUSIP));
        
        Assert.NotNull(actual.FilteredRecords);
        Assert.That(actual.FilteredRecords!.PageNumber, Is.EqualTo(filter.PageNumber));
        Assert.That(actual.FilteredRecords.PageSize, Is.EqualTo(filter.PageSize));
        Assert.True(actual.FilteredRecords.PagingEnabled);

        var actualItems = actual.FilteredRecords.Items.ToList();
        Assert.IsNotEmpty(actualItems);
        Assert.True(actualItems.TrueForAll(rec => items.Any(x => x.Id.Equals(rec.Id))));
        
        MockedService.Verify(mock => mock.FindByIdAsync(It.IsAny<long>()));
        MockedIndexRecordService.Verify(mock => mock.FetchFilteredMinimalAsync(
            It.IsAny<IFilter<IndexRecord>>(), It.IsAny<QueryParams>()));
    }
}
