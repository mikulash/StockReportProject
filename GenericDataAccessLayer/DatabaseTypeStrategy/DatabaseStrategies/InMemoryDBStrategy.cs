using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GenericDataAccessLayer.DatabaseTypeStrategy.DatabaseStrategies;

public class InMemoryDbStrategy(IConfiguration configuration) : BaseDbStrategy(configuration)
{
    private const string DatabaseName = "InMemDatabaseName";
    public override IServiceCollection AddDbContext<TDbContext>(IServiceCollection services)
    {
        services.AddDbContextFactory<TDbContext>(options => 
            options
                .UseInMemoryDatabase(Config.GetSection(DatabaseName).Value ?? Guid.NewGuid().ToString())
                .UseLazyLoadingProxies()
        );

        return services;
    }
}