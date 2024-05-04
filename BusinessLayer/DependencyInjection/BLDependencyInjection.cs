using BusinessLayer.DTOs.FundDTO.Create;
using BusinessLayer.DTOs.FundDTO.Update;
using BusinessLayer.DTOs.FundDTO.View;
using BusinessLayer.Facades;
using BusinessLayer.Facades.CompanyFacade;
using BusinessLayer.Facades.IndexRecordDiffFacade;
using BusinessLayer.Facades.IndexRecordFacade;
using BusinessLayer.Facades.ProcessFileFacade;
using BusinessLayer.Mappers;
using BusinessLayer.Services;
using BusinessLayer.Services.CompanyService;
using BusinessLayer.Services.IndexRecordService;
using BusinessLayer.Services.NullableIndexRecordService;
using DataAccessLayer.Models;
using FileLoader.FileParserStrategy;
using Infrastructure.Query.Filters.EntityFilters;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer.DependencyInjection;

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
        services.AddScoped<IGenericService<Fund, long>, GenericService<Fund, long>>();
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
    
    public static void RegisterBLDependecies(this IServiceCollection services)
    {
        RegisterExternal(services);
        RegisterMappers(services);
        RegisterServices(services);
        RegisterFacades(services);
    }
}