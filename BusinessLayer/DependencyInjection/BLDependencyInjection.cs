using BusinessLayer.DTOs.FundDTO;
using BusinessLayer.Facades;
using BusinessLayer.Mappers;
using BusinessLayer.Services;
using DataAccessLayer.Models;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer.DependencyInjection;

public static class BLDependencyInjection
{
    private static void RegisterMappers(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(FundProfile));

        services.AddAutoMapper(typeof(QueryMappingProfile));
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IGenericService<Fund, long>, GenericService<Fund, long>>();
    }

    private static void RegisterFacades(IServiceCollection services)
    {
        services.AddScoped<IGenericFacade<Fund, long, CreateFundDto, UpdateFundDto, ViewFundDto, ViewFundDto>, 
            GenericFacade<Fund, long, CreateFundDto, UpdateFundDto, ViewFundDto, ViewFundDto>>();
    }
    
    public static void RegisterBLDependecies(this IServiceCollection services)
    {
        RegisterMappers(services);
        RegisterServices(services);
        RegisterFacades(services);
    }
}