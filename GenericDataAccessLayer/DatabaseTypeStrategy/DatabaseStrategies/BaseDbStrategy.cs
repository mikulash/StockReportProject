using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GenericDataAccessLayer.DatabaseTypeStrategy.DatabaseStrategies;

public abstract class BaseDbStrategy(IConfiguration configuration)
{
    protected readonly IConfiguration Config = configuration;

    public abstract IServiceCollection AddDbContext<TDbContext>(IServiceCollection services) where TDbContext : DbContext;
}
