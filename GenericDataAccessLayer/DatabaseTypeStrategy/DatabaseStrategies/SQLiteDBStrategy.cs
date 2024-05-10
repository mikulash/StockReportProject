using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GenericDataAccessLayer.DatabaseTypeStrategy.DatabaseStrategies;

public class SQLiteDbStrategy(IConfiguration configuration) : IDBStrategy(configuration)
{
    private const string MigrationsProjectKey = "MigrationsProject";
    private const string ConnectionString = "DefaultConnection";

    public override IServiceCollection AddDbContext<TDbContext>(IServiceCollection services)
    {
        services.AddDbContextFactory<TDbContext>(options =>
        {
            options
                .UseSqlite(
                    Config.GetConnectionString(ConnectionString),
                    x => x.MigrationsAssembly(Config.GetSection(MigrationsProjectKey).Value ?? string.Empty)
                )
                .UseLazyLoadingProxies();
        });

        return services;
    }
}
