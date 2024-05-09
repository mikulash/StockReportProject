using System.Linq.Expressions;
using DataAccessLayer.Models;
using GenericBusinessLayer.Exceptions;
using GenericInfrastructure.Query;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StockBusinessLayer.Services.CompanyService;
using StockInfrastructure.Query.Filters.EntityFilters;
using TestUtilities;

namespace StockBusinessLayerTests.ServiceTests;

public class CompanyServiceTests : ServiceTestBase<Company>
{
    protected override ICompanyService GetService()
    {
        var serviceProvider = CreateServiceProvider();

        using var scope = serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<ICompanyService>();
    }
    
        [Test]
    public async Task CreateAsync_NewCompanyCorrectFormat_ReturnsCreatedCompany()
    {
        // arrange
        var expected = TestDataInitializer.GetTestCompany();
        
        MockedRepository
            .Setup(mock => mock.AddAsync(It.IsAny<Company>()))
            .Returns(Task.FromResult(expected))
            .Verifiable();
        
        // act
        var companyService = GetService();
        var actual = await companyService.CreateAsync(expected);
            
        // assert
        Assert.NotNull(actual);
        Assert.That(actual, Is.EqualTo(expected));
        
        MockedRepository.Verify(repo => repo.AddAsync(It.IsAny<Company>()));
        MockedUoW.Verify(uow => uow.CommitAsync());
    }

    [Test]
    public async Task CreateRangeAsync_NewCompaniesInCorrectFormat_ReturnsCreatedCompanies()
    {
        // arrange
        var expected = TestDataInitializer.GetTestCompanies().ToArray();

        MockedRepository
            .Setup(mock => mock.AddRangeAsync(It.IsAny<Company[]>()))
            .Returns(Task.FromResult(expected))
            .Verifiable();
        
        // act
        var companyService = GetService();
        var actual = (await companyService.CreateRangeAsync(expected)).ToArray();
            
        // assert
        Assert.NotNull(actual);
        Assert.That(actual.Length, Is.EqualTo(expected.Length));
        Assert.That(actual, Is.EquivalentTo(expected));
        
        MockedRepository.Verify(repo => repo.AddRangeAsync(It.IsAny<Company[]>()));
        MockedUoW.Verify(uow => uow.CommitAsync());
    }

    [TestCase("Best Company", "BEST", "3213654645P")]
    [TestCase("EL TESLA", "ETES", "31254312LS")]
    public async Task UpdateAsync_NewCompanyNameAndTickerAndCUSIP_ReturnsUpdatedCompany(string newCompanyName, 
        string newCompanyTicker, string newCompanyCUSIP)
    {
        // arrange
        var expected = TestDataInitializer.GetTestCompany();
        expected.CompanyName = newCompanyName;
        expected.Ticker = newCompanyTicker;
        expected.CUSIP = newCompanyCUSIP;
        
        MockedRepository
            .Setup(mock => mock.Update(It.IsAny<Company>()))
            .Verifiable();
        
        // act
        var companyService = GetService();
        var actual = await companyService.UpdateAsync(expected);
            
        // assert
        Assert.NotNull(actual);
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(actual.CompanyName, Is.EqualTo(newCompanyName));
        Assert.That(actual.Ticker, Is.EqualTo(newCompanyTicker));
        Assert.That(actual.CUSIP, Is.EqualTo(newCompanyCUSIP));
        
        MockedRepository.Verify(repo => repo.Update(It.IsAny<Company>()));
        MockedUoW.Verify(uow => uow.CommitAsync());
    }

    [Test]
    public async Task FetchAllAsync_NoRestrictions_ReturnsAllCompanies()
    {
        // arrange
        var expected = TestDataInitializer.GetTestCompanies();

        MockedRepository
            .Setup(mock => 
                mock.GetAllAsync(
                    It.IsAny<Expression<Func<Company, bool>>?>(), 
                    It.IsAny<Expression<Func<Company, object>>[]>()))
            .Returns(Task.FromResult((IEnumerable<Company>)expected))
            .Verifiable();
        
        // act
        var companyService = GetService();
        var actual = (await companyService.FetchAllAsync()).ToList();
        
        // assert
        Assert.IsNotEmpty(actual);
        Assert.That(actual.Count, Is.EqualTo(expected.Count));
        Assert.That(actual, Is.EquivalentTo(expected));
        
        MockedRepository.Verify(repo => repo.GetAllAsync(
            It.IsAny<Expression<Func<Company, bool>>?>(), 
            It.IsAny<Expression<Func<Company, object>>[]>()));
    }
    
    [Test]
    public async Task FetchFilteredAsync_CompanyFilterApplied_ReturnsFilteredCompanies()
    {
        // arrange
        string companyNameFilter = "INC";

        var expectedItems = TestDataInitializer.GetTestCompanies()
            .Where(comp => comp.CompanyName.Contains(companyNameFilter))
            .OrderBy(x => x.CompanyName)
            .ToList();
        CompanyFilter filter = new CompanyFilter { CONTAINS_CompanyName = companyNameFilter };
        
        QueryParams queryParams = new QueryParams
        {
            PageNumber = 1,
            PageSize = 10,
            SortAscending = true,
            SortParameter = "CompanyName"
        };

        QueryResult<Company> expected = new QueryResult<Company>()
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
        var companyService = GetService();
        var actual = await companyService.FetchFilteredAsync(filter, queryParams);
        
        // assert
        Assert.NotNull(actual);
        Assert.IsNotEmpty(actual.Items);
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(actual.Items, Is.EquivalentTo(expected.Items));
        Assert.That(actual.Items.Select(x => x.CompanyName), Is.Ordered.Ascending);
        
        MockedQuery.Verify(query => query.Where(It.IsAny<Expression<Func<Company, bool>>?>()));
        MockedQuery.Verify(query => query.CountTotalAsync());
        MockedQuery.Verify(query => query.ExecuteAsync());
        MockedQuery.Verify(query => query.Page(It.IsAny<int>(), It.IsAny<int>()));
        MockedQuery.Verify(query => query.SortBy(It.IsAny<string>(), It.IsAny<bool>()));
    }

    [Test]
    public async Task FindByIdAsync_CompanyNotNull_ReturnCompany()
    {
        // arrange
        var expected = TestDataInitializer.GetTestCompany();

        MockedRepository
            .Setup(mock => mock.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(expected)!)
            .Verifiable();
        
        // act
        var companyService = GetService();
        var actual = await companyService.FindByIdAsync(expected.Id);
        
        // assert
        Assert.NotNull(actual);
        Assert.That(actual, Is.EqualTo(expected));
        
        MockedRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<long>()));
    }

    [Test]
    public async Task FindByIdAsync_CompanyIsNull_ThrowsNoSuchEntityException()
    {
        // arrange
        var nonExistingId = 100000L;
        
        MockedRepository
            .Setup(mock => mock.GetByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult<Company>(null!)!);
        
        // act
        var companyService = GetService();
        Assert.ThrowsAsync<NoSuchEntityException<long>>(async () => await companyService.FindByIdAsync(nonExistingId));
        
        // assert
        MockedRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<long>()));
    }

    [TestCase(1L)]
    [TestCase(2L)]
    [TestCase(5L)]
    public async Task FindByIdAllIndexRecordsAsync_CompanyIsNotNull_ReturnsCompanyWithId(long companyId)
    {
        // arrange
        var expected = TestDataInitializer.GetTestCompanies().Find(x => x.Id.Equals(companyId));
        Assert.IsNotNull(expected);
        expected!.IndexRecords = TestDataInitializer.GetTestIndexRecords().Where(x => x.CompanyId.Equals(companyId));

        MockedRepository
            .Setup(mock => mock.GetByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<Company, object>>[]>()))
            .Returns(Task.FromResult(expected)!)
            .Verifiable();
        
        // act
        var companyService = GetService();
        var actual = await companyService.FindByIdAllIndexRecordsAsync(companyId);
        
        // assert
        Assert.That(actual.Id, Is.EqualTo(companyId));
        Assert.IsNotNull(actual.IndexRecords);
        Assert.That(actual.IndexRecords!.Count(), Is.EqualTo(expected.IndexRecords.Count()));
        Assert.IsTrue(actual.IndexRecords!.All(x => x.CompanyId.Equals(companyId)));
        
        MockedRepository.Verify(mock => mock.GetByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<Company, object>>[]>()));
    }

    [Test]
    public async Task FindByIdAllIndexRecordsAsync_CompanyIsNull_ThrowsNoSuchEntityException()
    {
        // arrange
        var nonExistingId = 100000L;
        
        MockedRepository
            .Setup(mock => mock.GetByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<Company, object>>[]>()))
            .Returns(Task.FromResult<Company>(null!)!);
        
        // act
        var companyService = GetService();
        Assert.ThrowsAsync<NoSuchEntityException<long>>(async () => await companyService.FindByIdAllIndexRecordsAsync(nonExistingId));
        
        // assert
        MockedRepository.Verify(mock => mock.GetByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<Company, object>>[]>()));
    }

    [Test]
    public async Task DeleteAsync_ExistingEntityToDelete_DeleteIsCalled()
    {
        // arrange
        var toDelete = TestDataInitializer.GetTestCompany();
        
        MockedRepository
            .Setup(mock => mock.Delete(It.IsAny<Company>()))
            .Verifiable();
        
        // act
        var companyService = GetService();
        await companyService.DeleteAsync(toDelete);
            
        // assert
        MockedRepository.Verify(repo => repo.Delete(It.IsAny<Company>()));
        MockedUoW.Verify(uow => uow.CommitAsync());
    }
    
    [Test]
    public async Task DeleteRangeAsync_ExistingEntitiesToDelete_DeleteIsCalled()
    {
        // arrange
        var toDelete = TestDataInitializer.GetTestCompanies().ToArray();
        
        MockedRepository
            .Setup(mock => mock.DeleteRange(It.IsAny<Company[]>()))
            .Verifiable();
        
        // act
        var companyService = GetService();
        await companyService.DeleteRangeAsync(toDelete);
            
        // assert
        MockedRepository.Verify(repo => repo.DeleteRange(It.IsAny<Company[]>()));
        MockedUoW.Verify(uow => uow.CommitAsync());
    }
}
