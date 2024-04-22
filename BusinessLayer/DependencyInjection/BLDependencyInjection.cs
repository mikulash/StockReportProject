using BusinessLayer.DTOs.FundDTO.Create;
using BusinessLayer.DTOs.FundDTO.Update;
using BusinessLayer.DTOs.FundDTO.View;
using BusinessLayer.Facades;
using BusinessLayer.Facades.CompanyFacade;
using BusinessLayer.Facades.IndexRecordFacade;
using BusinessLayer.Mappers;
using BusinessLayer.Services;
using BusinessLayer.Services.CompanyService;
using BusinessLayer.Services.IndexRecordService;
using DataAccessLayer.Models;
using Infrastructure.Query.Filters.EntityFilters;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer.DependencyInjection;

public static class BLDependencyInjection
{
    private static void RegisterMappers(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(FundProfile));
        services.AddAutoMapper(typeof(CompanyProfile));
        services.AddAutoMapper(typeof(IndexRecordProfile));

        services.AddAutoMapper(typeof(QueryMappingProfile));
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IGenericService<Fund, long>, GenericService<Fund, long>>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IIndexRecordService, IndexRecordService>();
    }

    private static void RegisterFacades(IServiceCollection services)
    {
        services.AddScoped<IGenericFacade<Fund, long, IGenericService<Fund, long>, CreateFundDto, UpdateFundDto, ViewFundDto, ViewFundDto, FundFilter>, 
            GenericFacade<Fund, long, IGenericService<Fund, long>, CreateFundDto, UpdateFundDto, ViewFundDto, ViewFundDto, FundFilter>>();
        services.AddScoped<ICompanyFacade, CompanyFacade>();
        services.AddScoped<IIndexRecordFacade, IndexRecordFacade>();
    }
    
    public static void RegisterBLDependecies(this IServiceCollection services)
    {
        RegisterMappers(services);
        RegisterServices(services);
        RegisterFacades(services);
    }
}