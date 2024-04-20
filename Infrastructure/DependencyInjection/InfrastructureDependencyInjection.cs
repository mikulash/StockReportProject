using DataAccessLayer.Models;
using Infrastructure.Query;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class InfrastructureDependencyInjection
{
    public static void RegisterInfrastructureDependencies(this IServiceCollection services)
    {
        services.AddScoped<IGenericRepository<Company, long>, GenericRepository<Company, long>>();
        services.AddScoped<IGenericRepository<Fund, long>, GenericRepository<Fund, long>>();
        services.AddScoped<IGenericRepository<IndexRecord, long>, GenericRepository<IndexRecord, long>>();

        services.AddScoped<IUnitOfWork, StockUnitOfWork>();
        
        services.AddScoped<IQuery<Company, long>, QueryBase<Company, long>>();
        services.AddScoped<IQuery<Fund, long>, QueryBase<Fund, long>>();
        services.AddScoped<IQuery<IndexRecord, long>, QueryBase<IndexRecord, long>>();
    }
}
