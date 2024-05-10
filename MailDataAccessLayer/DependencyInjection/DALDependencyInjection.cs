using GenericDataAccessLayer.DatabaseTypeStrategy;
using MailDataAccessLayer.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MailDataAccessLayer.DependencyInjection;

public static class DALDependencyInjection
{
    public static void RegisterDALDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        DatabaseTypeContext databaseTypeContext = new DatabaseTypeContext(EnumToStrategyConverter.CreateStrategy(configuration));
        databaseTypeContext.AddDbContext<MailDbContext>(services);
    }

}
