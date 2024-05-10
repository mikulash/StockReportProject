using Microsoft.Extensions.DependencyInjection;
using StockAPI.DTOs.CompanyDTOs.Create;
using StockBusinessLayer.Exceptions;
using StockBusinessLayer.Services.NullableIndexRecordService;
using TestUtilities;

namespace StockBusinessLayerTests.ServiceTests;

public class NullableIndexRecordServiceTests
{
    private MockedDependencyInjectionBuilder _serviceProviderBuilder = null!;

    [SetUp]
    public void Initialize()
    {
        _serviceProviderBuilder = new MockedDependencyInjectionBuilder()
            .AddBusinessLayer();
    }

    private INullableIndexRecordService GetService()
    {
        var serviceProvider = _serviceProviderBuilder.Create();
        
        using var scope = serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<INullableIndexRecordService>();
    }

    [Test]
    public void ApplyFilter_NoMissingRequiredDataInInput_PropertiesPopulated()
    {
        // arrange
        var indexRecordList = TestDataInitializer.GetTestNullableIndexRecords();
        
        var expectedDate = indexRecordList.First().Date;
        Assert.NotNull(expectedDate);

        var expectedFundName = indexRecordList.First().Fund;
        Assert.NotNull(expectedFundName);

        var expectedCompanies = indexRecordList
            .Select(x => new CreateCompanyDto { CompanyName = x.Company, CUSIP = x.CUSIP, Ticker = x.Ticker})
            .ToList();
        
        // act
        var service = GetService();
        service.ApplyFilter(indexRecordList);

        // assert
        Assert.NotNull(service.IndexRecordList);
        Assert.That(service.IndexRecordList, Is.EquivalentTo(indexRecordList));
        
        Assert.That(service.Date, Is.EqualTo(expectedDate));
        Assert.That(service.FundName, Is.EqualTo(expectedFundName));
        service.CompanyList
            .ForEach(comp => 
                Assert.NotNull(expectedCompanies.Find(x 
                    => x.CompanyName.Equals(comp.CompanyName))));
    }

    [Test]
    public void ApplyFilter_MultipleDifferentDates_ThrowsInvalidRecordsException()
    {
        // arrange
        var indexRecordList = TestDataInitializer.GetTestNullableIndexRecords();
        indexRecordList.First().Date = new DateOnly(2000, 1, 1);
        
        // act & assert
        var service = GetService();
        Assert.Throws<InvalidRecordsException>(() => service.ApplyFilter(indexRecordList));
    }

    [Test]
    public void ApplyFilter_AllDatesAreNull_ThrowsInvalidRecordsException()
    {
        // arrange
        var indexRecordList = TestDataInitializer.GetTestNullableIndexRecords();
        indexRecordList.ForEach(x => x.Date = null);
        
        // act & assert
        var service = GetService();
        Assert.Throws<InvalidRecordsException>(() => service.ApplyFilter(indexRecordList));
    }

    [Test]
    public void ApplyFilter_MultipleFundNames_ThrowsInvalidRecordsException()
    {
        // arrange
        var indexRecordList = TestDataInitializer.GetTestNullableIndexRecords();
        var differentFundName = $"{indexRecordList.First().Fund}AndMore";
        indexRecordList.First().Fund = differentFundName;

        // act & assert
        var service = GetService();
        Assert.Throws<InvalidRecordsException>(() => service.ApplyFilter(indexRecordList));
    }

    [Test]
    public void ApplyFilter_AllNullFunds_ThrowsInvalidRecordsException()
    {
        // arrange
        var indexRecordList = TestDataInitializer.GetTestNullableIndexRecords();
        indexRecordList.ForEach(x => x.Fund = null);
        
        // act & assert
        var service = GetService();
        Assert.Throws<InvalidRecordsException>(() => service.ApplyFilter(indexRecordList));
    }

    [Test]
    public void ApplyFilter_FirstCompanyHasNullCUSIP_CompaniesWithoutTheFirst()
    {
        // arrange
        var indexRecordList = TestDataInitializer.GetTestNullableIndexRecords();
        var changedCompany = indexRecordList.First();
        changedCompany.CUSIP = null;
        
        // act
        var service = GetService();
        service.ApplyFilter(indexRecordList);
        
        // assert
        Assert.NotNull(service.CompanyList);
        Assert.True(service.CompanyList.TrueForAll(x => !x.CompanyName.Equals(changedCompany.Company)));
    }
}
