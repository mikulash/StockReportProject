using DataAccessLayer.Models;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StockBusinessLayer.Facades.IndexRecordDiffFacade;
using StockBusinessLayer.Services.IndexRecordService;
using TestUtilities;

namespace StockBusinessLayerTests.FacadeTests;

public class IndexRecordDiffFacadeTests
{
    private MockedDependencyInjectionBuilder _serviceProviderBuilder = null!;
    private Mock<IIndexRecordService> _mockedIndexRecordService = null!;
    
    [SetUp]
    public void Initialize()
    {
        _serviceProviderBuilder = new MockedDependencyInjectionBuilder()
            .AddBusinessLayer();
        
        _mockedIndexRecordService = new Mock<IIndexRecordService>();
    }
    
    protected virtual ServiceProvider CreateServiceProvider() =>
        _serviceProviderBuilder
            .AddScoped(_mockedIndexRecordService.Object)
            .Create();
    
    protected IIndexRecordDiffFacade GetFacade()
    {
        var serviceProvider = CreateServiceProvider();

        using var scope = serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IIndexRecordDiffFacade>();
    }

    private void SetFundAndCompanies(Fund fund, List<IndexRecord> list)
    {
        foreach (var item in list)
        {
            item.Fund = fund;
            item.Company = TestDataInitializer.GetTestCompanies().First(comp => comp.Id.Equals(item.CompanyId));
        }
    }

    [Test]
    public async Task GetIndexRecordDifferenceAsync_ValidFundNameAndDate_ReturnDifference()
    {
        // arrange
        var fund = TestDataInitializer.GetTestFund();
        var currentDate = new DateOnly(2024, 1, 2);
        var current = TestDataInitializer.GetTestIndexRecords()
            .Where(rec => rec.IssueDate.Equals(currentDate))
            .ToList();
        SetFundAndCompanies(fund, current);
        
        var lastDate = new DateOnly(2024, 1, 1);
        var last = TestDataInitializer.GetTestIndexRecords()
            .Where(rec => rec.IssueDate.Equals(lastDate))
            .ToList();
        SetFundAndCompanies(fund, last);
        
        _mockedIndexRecordService
            .Setup(mock => mock.FetchByDateAndFundNameAsync(It.IsAny<string>(), currentDate))
            .Returns(Task.FromResult((IEnumerable<IndexRecord>)current))
            .Verifiable();
        
        _mockedIndexRecordService
            .Setup(mock => mock.FetchByDateAndFundNameAsync(It.IsAny<string>(), lastDate))
            .Returns(Task.FromResult((IEnumerable<IndexRecord>)last))
            .Verifiable();
        
        _mockedIndexRecordService
            .Setup(mock => mock.FetchComparableOlderDateAsync(It.IsAny<string>(), It.IsAny<DateOnly>()))
            .Returns(Task.FromResult((DateOnly?)lastDate))
            .Verifiable();
        
        // act
        var facade = GetFacade();
        var actual = await facade.GetIndexRecordDifferenceAsync(fund.FundName, currentDate);

        // assert
        Assert.NotNull(actual.DiffRecords);
        Assert.IsNotEmpty(actual.DiffRecords!);
        Assert.That(actual.DiffRecords.Count, Is.EqualTo(last.Count));
        Assert.That(actual.DiffRecords.Count, Is.EqualTo(current.Count));

        _mockedIndexRecordService
            .Verify(mock => mock.FetchByDateAndFundNameAsync(It.IsAny<string>(), currentDate)); 
        _mockedIndexRecordService
            .Verify(mock => mock.FetchByDateAndFundNameAsync(It.IsAny<string>(), lastDate));
        _mockedIndexRecordService
            .Verify(mock => mock.FetchComparableOlderDateAsync(It.IsAny<string>(), It.IsAny<DateOnly>()));
    }

    [Test]
    public async Task GetIndexRecordDifferenceAsync_ValidFundNameAndDateNoOldEntries_ReturnOnlyNewDifference()
    {
        // arrange
        var fund = TestDataInitializer.GetTestFund();
        var currentDate = new DateOnly(2024, 1, 2);
        var current = TestDataInitializer.GetTestIndexRecords()
            .Where(rec => rec.IssueDate.Equals(currentDate))
            .ToList();
        SetFundAndCompanies(fund, current);
        
        _mockedIndexRecordService
            .Setup(mock => mock.FetchByDateAndFundNameAsync(It.IsAny<string>(), currentDate))
            .Returns(Task.FromResult((IEnumerable<IndexRecord>)current))
            .Verifiable();
        
        _mockedIndexRecordService
            .Setup(mock => mock.FetchComparableOlderDateAsync(It.IsAny<string>(), It.IsAny<DateOnly>()))
            .Returns(Task.FromResult<DateOnly?>(null))
            .Verifiable();
        
        // act
        var facade = GetFacade();
        var actual = await facade.GetIndexRecordDifferenceAsync(fund.FundName, currentDate);
        
        // assert
        Assert.NotNull(actual.DiffRecords);
        Assert.IsNotEmpty(actual.DiffRecords!);
        Assert.That(actual.DiffRecords.Count, Is.EqualTo(current.Count));
        Assert.True(actual.DiffRecords.TrueForAll(diff => diff.IsNew));

        _mockedIndexRecordService
            .Verify(mock => mock.FetchByDateAndFundNameAsync(It.IsAny<string>(), currentDate)); 
        _mockedIndexRecordService
            .Verify(mock => mock.FetchComparableOlderDateAsync(It.IsAny<string>(), It.IsAny<DateOnly>()));
    }
}