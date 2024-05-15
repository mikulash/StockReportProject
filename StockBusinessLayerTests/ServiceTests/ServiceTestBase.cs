using GenericBusinessLayer.Services;
using GenericDataAccessLayer.Models;
using GenericInfrastructure.Query;
using GenericInfrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StockInfrastructure.UnitOfWork;
using TestUtilities;

namespace StockBusinessLayerTests.ServiceTests;

public class ServiceTestBase<TEntity> where TEntity : BaseEntity<long>
{
    protected MockedDependencyInjectionBuilder ServiceProviderBuilder = null!;

    protected Mock<IGenericRepository<TEntity, long>> MockedRepository = null!;
    protected Mock<IQuery<TEntity, long>> MockedQuery = null!;
    protected Mock<IStockUnitOfWork> MockedUoW = null!;
    
    [SetUp]
    public void Initialize()
    {
        ServiceProviderBuilder = new MockedDependencyInjectionBuilder()
            .AddInfrastructure()
            .AddBusinessLayer();

        MockedRepository = new();
        MockedQuery = new();
        MockedUoW = new();
       
        BusinessLayerTestUtilities.InitializeQuery<TEntity>(MockedQuery);
        BusinessLayerTestUtilities.InitializeUoW<TEntity>(MockedUoW, MockedRepository, MockedQuery);
    }
    
    protected virtual ServiceProvider CreateServiceProvider() =>
        ServiceProviderBuilder
            .AddScoped(MockedRepository.Object)
            .AddScoped(MockedQuery.Object)
            .AddScoped(MockedUoW.Object)
            .Create();
    
    protected virtual IGenericService<TEntity, long> GetService()
    {
        var serviceProvider = CreateServiceProvider();

        using var scope = serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IGenericService<TEntity, long>>();
    }
}