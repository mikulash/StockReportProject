using GenericDataAccessLayer.DatabaseTypeStrategy.DatabaseStrategies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenericDataAccessLayer.DatabaseTypeStrategy;

public class DatabaseTypeContext(IDBStrategy strategy)
{
    public IDBStrategy DatabaseStrategy { get; set; } = strategy;

    public void AddDbContext<TDbContext>(IServiceCollection serviceCollection) 
        where TDbContext : DbContext 
        => DatabaseStrategy.AddDbContext<TDbContext>(serviceCollection);
}
