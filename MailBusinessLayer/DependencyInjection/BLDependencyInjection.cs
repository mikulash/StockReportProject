using GenericBusinessLayer.Facades;
using GenericBusinessLayer.Mappers;
using GenericBusinessLayer.Services;
using MailAPI.DTOs.MailSubscriberDTOs.Create;
using MailAPI.DTOs.MailSubscriberDTOs.Update;
using MailAPI.DTOs.MailSubscriberDTOs.View;
using MailBusinessLayer.Mappers;
using MailDataAccessLayer.Models;
using MailInfrastructure.EntityFilters;
using MailInfrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace MailBusinessLayer.DependencyInjection;

public static class BLDependencyInjection
{
    private static void RegisterMappers(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MailSubscriberProfile));

        services.AddAutoMapper(typeof(QueryMappingProfile));
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IGenericService<MailSubscriber, long>, GenericService<MailSubscriber, long, IMailUnitOfWork>>();
    }

    private static void RegisterFacades(IServiceCollection services)
    {
        services.AddScoped
        <
            IGenericFacade<MailSubscriber, long, IGenericService<MailSubscriber, long>, CreateMailSubscriberDto,
                UpdateMailSubscriberDto, ViewMailSubscriberDto, ViewMailSubscriberDto, MailSubscriberFilter>,
            GenericFacade<MailSubscriber, long, IGenericService<MailSubscriber, long>, CreateMailSubscriberDto,
                UpdateMailSubscriberDto, ViewMailSubscriberDto, ViewMailSubscriberDto, MailSubscriberFilter>
        >();
    }
    
    public static void RegisterBLDependecies(this IServiceCollection services)
    {
        RegisterMappers(services);
        RegisterServices(services);
        RegisterFacades(services);
    }
}
