using Microsoft.Extensions.DependencyInjection;
using StockBusinessLayer.DependencyInjection;
using StockInfrastructure.DependencyInjection;

namespace TestUtilities;

public class MockedDependencyInjectionBuilder
{
    private IServiceCollection _serviceCollection = new ServiceCollection();

    public MockedDependencyInjectionBuilder AddInfrastructure()
    {
        _serviceCollection.RegisterInfrastructureDependencies();
        return this;
    }

    public MockedDependencyInjectionBuilder AddBusinessLayer()
    {
        _serviceCollection.RegisterBLDependencies();
        return this;
    }

    public MockedDependencyInjectionBuilder AddScoped<T>(T objectToRegister)
        where T : class
    {
        _serviceCollection = _serviceCollection
            .AddScoped<T>(_ => objectToRegister);

        return this;
    }

    public ServiceProvider Create() => _serviceCollection.BuildServiceProvider();
}
