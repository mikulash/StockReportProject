using Microsoft.Extensions.DependencyInjection;
using Moq;
using TestUtilities;

namespace StockBusinessLayerTests.FacadeTests;

public class FacadeTestBase<TService> where TService : class
{
    protected MockedDependencyInjectionBuilder ServiceProviderBuilder = null!;
    protected Mock<TService> MockedService = null!;
    
    [SetUp]
    public virtual void Initialize()
    {
        ServiceProviderBuilder = new MockedDependencyInjectionBuilder()
            .AddBusinessLayer();

        MockedService = new Mock<TService>();
    }

    protected virtual ServiceProvider CreateServiceProvider() =>
        ServiceProviderBuilder
            .AddScoped(MockedService.Object)
            .Create();
    
    protected TFacade GetFacade<TFacade>() where TFacade : notnull
    {
        var serviceProvider = CreateServiceProvider();

        using var scope = serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<TFacade>();
    }
}
