using DataAccessLayer.Models;
using GenericBusinessLayer.Exceptions;
using GenericBusinessLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StockAPI.DTOs.IndexRecordDTOs.Create;
using StockAPI.DTOs.IndexRecordDTOs.Update;
using StockAPI.DTOs.IndexRecordDTOs.View;
using StockBusinessLayer.Facades.IndexRecordFacade;
using StockBusinessLayer.Services.CompanyService;
using StockBusinessLayer.Services.IndexRecordService;
using TestUtilities;

namespace StockBusinessLayerTests.FacadeTests;

public class IndexRecordFacadeTests : FacadeTestBase<IIndexRecordService>
{
    protected Mock<IGenericService<Fund, long>> MockedFundService = null!;
    protected Mock<ICompanyService> MockedCompanyService = null!;
    
    [SetUp]
    public override void Initialize()
    {
        base.Initialize();
        MockedFundService = new Mock<IGenericService<Fund, long>>();
        MockedCompanyService = new Mock<ICompanyService>();
    }

    protected override ServiceProvider CreateServiceProvider()
    {
        ServiceProviderBuilder
            .AddScoped(MockedFundService.Object)
            .AddScoped(MockedCompanyService.Object);
        return base.CreateServiceProvider();
    }

    private void AssertEqualEntities(DetailedViewIndexRecordDto actual, IndexRecord expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.Fund.FundName, Is.EqualTo(expected.Fund.FundName));
        Assert.That(actual.Company.CompanyName, Is.EqualTo(expected.Company.CompanyName));
        Assert.That(actual.Shares, Is.EqualTo(expected.Shares));
        Assert.That(actual.IssueDate, Is.EqualTo(expected.IssueDate));
        Assert.That(actual.Weight, Is.EqualTo(expected.Weight));
        Assert.That(actual.MarketValue, Is.EqualTo(expected.MarketValue));
    }

    private void MockFindByIdInAdditionalServices(Fund fund, Company company)
    {
        MockedFundService
            .Setup(mock => mock.FindByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(fund))
            .Verifiable();

        MockedCompanyService
            .Setup(mock => mock.FindByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(company))
            .Verifiable();
    }
    
    [Test]
    public async Task CreateAsync_NewEntity_ReturnsCreatedEntity()
    {
        // arrange
        var record = TestDataInitializer.GetTestIndexRecord();
        
        var company = TestDataInitializer.GetTestCompanies().First(x => x.Id.Equals(record.CompanyId));
        Assert.NotNull(company);
        var fund = TestDataInitializer.GetTestFunds().First(x => x.Id.Equals(record.FundId));
        Assert.NotNull(fund);

        record.Fund = fund;
        record.Company = company;
        
        var create = new CreateIndexRecordDto
        {
            CompanyId = record.CompanyId, FundId = record.FundId, Shares = record.Shares, Weight = record.Shares,
            IssueDate = record.IssueDate, MarketValue = record.MarketValue
        };
        
        MockedService
            .Setup(mock => mock.CreateAsync(It.IsAny<IndexRecord>(), It.IsAny<bool>()))
            .Returns(Task.FromResult(record))
            .Verifiable();

        MockFindByIdInAdditionalServices(fund, company);
        
        // act
        var facade = GetFacade<IIndexRecordFacade>();
        var actual = await facade.CreateAsync(create);
        
        // assert
        Assert.NotNull(actual);
        AssertEqualEntities(actual, record);
        
        MockedService.Verify(service => service.CreateAsync(It.IsAny<IndexRecord>(), It.IsAny<bool>()));
        MockedFundService.Verify(mock => mock.FindByIdAsync(It.IsAny<long>()));
        MockedCompanyService.Verify(mock => mock.FindByIdAsync(It.IsAny<long>()));
    }

    [Test]
    public async Task UpdateAsync_ValidEntityFound_ReturnsUpdatedEntity()
    {
        // arrange
        var record = TestDataInitializer.GetTestIndexRecord();
        record.Shares = 10;
        record.Weight = 10;
        record.MarketValue = 10;

        var update = new UpdateIndexRecordDto
        {
            FundId = record.FundId, CompanyId = record.CompanyId, Shares = record.Shares, Weight = record.Weight,
            IssueDate = record.IssueDate, MarketValue = record.MarketValue
        };
        
        var company = TestDataInitializer.GetTestCompanies().First(x => x.Id.Equals(record.CompanyId));
        Assert.NotNull(company);
        var fund = TestDataInitializer.GetTestFunds().First(x => x.Id.Equals(record.FundId));
        Assert.NotNull(fund);
        
        record.Fund = fund;
        record.Company = company;

        MockedService
            .Setup(mock => mock.FindByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(record))
            .Verifiable();

        MockedService
            .Setup(mock => mock.UpdateAsync(It.IsAny<IndexRecord>(), It.IsAny<bool>()))
            .Returns(Task.FromResult(record))
            .Verifiable();
        
        // act
        var facade = GetFacade<IIndexRecordFacade>();
        var actual = await facade.UpdateAsync(record.Id, update);
        
        // assert
        Assert.NotNull(actual);
        AssertEqualEntities(actual, record);
        
        MockedService.Verify(mock => mock.FindByIdAsync(It.IsAny<long>()));
        MockedService.Verify(mock => mock.UpdateAsync(It.IsAny<IndexRecord>(), It.IsAny<bool>()));
    }

    [Test]
    public async Task FindByIdAsync_EntityFound_ReturnsFoundEntity()
    {
        // arrange
        var record = TestDataInitializer.GetTestIndexRecord();
        
        var company = TestDataInitializer.GetTestCompanies().First(x => x.Id.Equals(record.CompanyId));
        Assert.NotNull(company);
        var fund = TestDataInitializer.GetTestFunds().First(x => x.Id.Equals(record.FundId));
        Assert.NotNull(fund);

        record.Fund = fund;
        record.Company = company;
        
        MockedService
            .Setup(mock => mock.FindByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(record))
            .Verifiable();
        
        // act
        var facade = GetFacade<IIndexRecordFacade>();
        var actual = await facade.FindByIdAsync(fund.Id);
        
        // assert
        Assert.NotNull(actual);
        AssertEqualEntities(actual, record);
        
        MockedService.Verify(mock => mock.FindByIdAsync(It.IsAny<long>()));
    }
    
    [Test]
    public void FindByIdAsync_EntityNotFound_ThrowsNoSuchEntityException()
    {
        // arrange
        var nonExistingId = 100000L;
    
        MockedService
            .Setup(mock => mock.FindByIdAsync(It.IsAny<long>()))
            .ThrowsAsync(new NoSuchEntityException<long>(typeof(IndexRecord)));
        
        // act
        var facade = GetFacade<IIndexRecordFacade>();
        
        // assert
        Assert.ThrowsAsync<NoSuchEntityException<long>>(async () => await facade.FindByIdAsync(nonExistingId));
        
        MockedService.Verify(mock => mock.FindByIdAsync(It.IsAny<long>()));
    }
    
    [Test]
    public async Task DeleteByIdAsync_EntityFound_DeleteCalled()
    {
        // arrange
        var record = TestDataInitializer.GetTestIndexRecord();
        
        MockedService
            .Setup(mock => mock.FindByIdAsync(It.IsAny<long>()))
            .Returns(Task.FromResult(record))
            .Verifiable();
        
        MockedService
            .Setup(mock => mock.DeleteAsync(It.IsAny<IndexRecord>(), It.IsAny<bool>()))
            .Verifiable();
        
        // act
        var facade = GetFacade<IIndexRecordFacade>();
        await facade.DeleteByIdAsync(record.Id);
        
        // assert
        MockedService.Verify(mock => mock.FindByIdAsync(It.IsAny<long>()));
        MockedService.Verify(mock => mock.DeleteAsync(It.IsAny<IndexRecord>(), It.IsAny<bool>()));
    }

    [Test]
    public async Task DeleteByDateAndFundAsync_FundNameAndDateProvided_DeleteRangeCalled()
    {
        // arrange
        var fundName = "Something";
        var date = TestDataInitializer.GetTestIndexRecord().IssueDate;
        var items = TestDataInitializer.GetTestIndexRecords();

        MockedService
            .Setup(mock => mock.FetchByDateAndFundNameAsync(It.IsAny<string>(), It.IsAny<DateOnly>()))
            .Returns(Task.FromResult((IEnumerable<IndexRecord>)items))
            .Verifiable();
        
        MockedService
            .Setup(mock => mock.DeleteRangeAsync(It.IsAny<IndexRecord[]>(), It.IsAny<bool>()))
            .Verifiable();
        
        // act
        var facade = GetFacade<IIndexRecordFacade>();
        await facade.DeleteByDateAndFundAsync(fundName, date);
        
        // assert
        MockedService.Verify(mock => mock.FetchByDateAndFundNameAsync(It.IsAny<string>(), It.IsAny<DateOnly>()));
        MockedService.Verify(mock => mock.DeleteRangeAsync(It.IsAny<IndexRecord[]>(), It.IsAny<bool>()));
    }
}