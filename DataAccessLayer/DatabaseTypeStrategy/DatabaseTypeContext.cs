using DataAccessLayer.DatabaseTypeStrategy.DatabaseStrategies;
using Microsoft.Extensions.DependencyInjection;
 
namespace DataAccessLayer.DatabaseTypeStrategy;

public class DatabaseTypeContext(IDBStrategy strategy)
{
    public IDBStrategy DatabaseStrategy { get; set; } = strategy;

    public void AddDbContext(IServiceCollection serviceCollection) => DatabaseStrategy.AddDbContext(serviceCollection);
}
