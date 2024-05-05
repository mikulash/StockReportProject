using DataAccessLayer.Models;
using GenericBusinessLayer.Services;
using GenericInfrastructure.Repository;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using TestUtilities;
using Moq;

namespace BusinessLayerTests.ServiceTests;

public class FundServiceTests
{
    private MockedDependencyInjectionBuilder _serviceProviderBuilder = null!;

    private Mock<IGenericRepository<Fund, long>> _mockedRepository = null!;

    private Mock<IStockUnitOfWork> _mockedUoW = null!;

    [SetUp]
    public void Initialize()
    {
       _serviceProviderBuilder = new MockedDependencyInjectionBuilder()
           .AddInfrastructure()
           .AddBusinessLayer();

       _mockedRepository = new();
       _mockedUoW = new();
       _mockedUoW.Setup(mock => mock.GetRepositoryByEntity<Fund, long>()).Returns(_mockedRepository.Object);
    }

    private ServiceProvider CreateServiceProvider() =>
        _serviceProviderBuilder
            .AddScoped(_mockedRepository.Object)
            .AddScoped(_mockedUoW.Object)
            .Create();

    [Test]
    public async Task CreateFund_NewFundCorrectFormat_ReturnsCreatedFund()
    {
        // arrange
        var fundToCreate = TestDataInitializer.GetTestFund();
        
        _mockedRepository
            .Setup(mock => mock.AddAsync(It.IsAny<Fund>()))
            .Returns(Task.FromResult(fundToCreate));
        
        // act
        var serviceProvider = CreateServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var fundService = scope.ServiceProvider.GetRequiredService<IGenericService<Fund, long>>();
        var result = await fundService.CreateAsync(fundToCreate);
            
        // assert
        Assert.NotNull(result);
        Assert.That(result, Is.EqualTo(fundToCreate));
    }
}
