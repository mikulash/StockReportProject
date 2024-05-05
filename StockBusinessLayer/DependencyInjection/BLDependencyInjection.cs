using DataAccessLayer.Models;
using FileLoader.FileParserStrategy;
using GenericBusinessLayer.Facades;
using GenericBusinessLayer.Mappers;
using GenericBusinessLayer.Services;
using StockInfrastructure.Query.Filters.EntityFilters;
using StockInfrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using StockAPI.DTOs.FundDTO.Create;
using StockAPI.DTOs.FundDTO.Update;
using StockAPI.DTOs.FundDTO.View;
using StockBusinessLayer.Facades.CompanyFacade;
using StockBusinessLayer.Facades.IndexRecordDiffFacade;
using StockBusinessLayer.Facades.IndexRecordFacade;
using StockBusinessLayer.Facades.ProcessFileFacade;
using StockBusinessLayer.Mappers;
using StockBusinessLayer.Services.CompanyService;
using StockBusinessLayer.Services.IndexRecordService;
using StockBusinessLayer.Services.NullableIndexRecordService;

namespace StockBusinessLayer.DependencyInjection;

public static class BLDependencyInjection
{
    private static void RegisterExternal(IServiceCollection services)
    {
        services.AddScoped<IParserMiddleware, ParserMiddleware>();
    }
    
    private static void RegisterMappers(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(FundProfile));
        services.AddAutoMapper(typeof(CompanyProfile));
        services.AddAutoMapper(typeof(IndexRecordProfile));

        services.AddAutoMapper(typeof(QueryMappingProfile));
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IGenericService<Fund, long>, GenericService<Fund, long, IStockUnitOfWork>>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IIndexRecordService, IndexRecordService>();
        services.AddScoped<INullableIndexRecordService, NullableIndexRecordService>();
    }

    private static void RegisterFacades(IServiceCollection services)
    {
        services.AddScoped<IGenericFacade<Fund, long, IGenericService<Fund, long>, CreateFundDto, UpdateFundDto, ViewFundDto, ViewFundDto, FundFilter>, 
            GenericFacade<Fund, long, IGenericService<Fund, long>, CreateFundDto, UpdateFundDto, ViewFundDto, ViewFundDto, FundFilter>>();
        services.AddScoped<ICompanyFacade, CompanyFacade>();
        services.AddScoped<IIndexRecordFacade, IndexRecordFacade>();
        services.AddScoped<IProcessFileFacade, ProcessFileFacade>();
        services.AddScoped<IIndexRecordDiffFacade, IndexRecordDiffFacade>();
    }
    
    public static void RegisterBLDependencies(this IServiceCollection services)
    {
        RegisterExternal(services);
        RegisterMappers(services);
        RegisterServices(services);
        RegisterFacades(services);
    }
}