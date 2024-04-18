using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.DatabaseTypeStrategy.DatabaseStrategies;

public abstract class IDBStrategy(IConfiguration configuration)
{
    protected readonly IConfiguration Config = configuration;

    public abstract IServiceCollection AddDbContext(IServiceCollection services);
}
