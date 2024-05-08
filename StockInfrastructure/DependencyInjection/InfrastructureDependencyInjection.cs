using DataAccessLayer.Data;
using DataAccessLayer.Models;
using GenericInfrastructure.Query;
using GenericInfrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using StockInfrastructure.UnitOfWork;

namespace StockInfrastructure.DependencyInjection;

public static class InfrastructureDependencyInjection
{
    public static void RegisterInfrastructureDependencies(this IServiceCollection services)
    {
        services.AddScoped<IGenericRepository<Company, long>, GenericRepository<Company, long, StockDbContext>>();
        services.AddScoped<IGenericRepository<Fund, long>, GenericRepository<Fund, long, StockDbContext>>();
        services.AddScoped<IGenericRepository<IndexRecord, long>, GenericRepository<IndexRecord, long, StockDbContext>>();

        services.AddScoped<IStockUnitOfWork, StockUnitOfWork>();
        
        services.AddScoped<IQuery<Company, long>, QueryBase<Company, long, IStockUnitOfWork>>();
        services.AddScoped<IQuery<Fund, long>, QueryBase<Fund, long, IStockUnitOfWork>>();
        services.AddScoped<IQuery<IndexRecord, long>, QueryBase<IndexRecord, long, IStockUnitOfWork>>();
    }
}
