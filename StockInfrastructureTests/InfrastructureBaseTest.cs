using Microsoft.Extensions.DependencyInjection;
using TestUtilities;

namespace StockInfrastructureTests;

public class InfrastructureBaseTest
{
    protected MockedDependencyInjectionBuilder ServiceProviderBuilder = null!;
    protected IServiceScope ServiceScope = null!;

    [OneTimeSetUp]
    public virtual void InitializeOnce()
    {
        ServiceProviderBuilder = InfrastructureTestUtilities.CreateMockedDependencyInjectionBuilder();
    }

    [SetUp]
    public virtual async Task Initialize()
    {
        ServiceScope =
            await InfrastructureTestUtilities.CreateServiceScopeAndInitializeDatabaseAsync(ServiceProviderBuilder);
    }

    [TearDown]
    public virtual void AfterEach()
    {
        ServiceScope.Dispose();
    }
}
