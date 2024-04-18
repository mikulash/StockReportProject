using DataAccessLayer.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.DatabaseTypeStrategy.DatabaseStrategies;

public class SQLiteDbStrategy(IConfiguration configuration) : IDBStrategy(configuration)
{
    private const string MigrationsProject = "DAL.SQLite.Migrations";
    private const string ConnectionString = "DefaultConnection";

    public override IServiceCollection AddDbContext(IServiceCollection services)
    {
        services.AddDbContextFactory<StockDbContext>(options =>
        {
            options
                .UseSqlite(
                    Config.GetConnectionString(ConnectionString),
                    x => x.MigrationsAssembly(MigrationsProject)
                )
                .UseLazyLoadingProxies();
        });

        return services;
    }
}
