using DataAccessLayer.Data;
using GenericDataAccessLayer.DatabaseTypeStrategy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.DependencyInjection;

public static class DALDependencyInjection
{
    public static void RegisterDALDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        DatabaseTypeContext databaseTypeContext = new DatabaseTypeContext(EnumToStrategyConverter.CreateStrategy(configuration));
        databaseTypeContext.AddDbContext<StockDbContext>(services);
    }

}
