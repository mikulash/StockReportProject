using GenericBusinessLayer.Facades;
using GenericBusinessLayer.Mappers;
using GenericBusinessLayer.Services;
using MailAPI.DTOs.MailSubscriberDTOs.Create;
using MailAPI.DTOs.MailSubscriberDTOs.Update;
using MailAPI.DTOs.MailSubscriberDTOs.View;
using MailBusinessLayer.Facades.MailFacade;
using MailBusinessLayer.Mappers;
using MailBusinessLayer.Services;
using MailBusinessLayer.Services.MailService;
using MailBusinessLayer.Facades.MailSubscriberFacade;
using MailBusinessLayer.Scheduler;
using MailBusinessLayer.Services.MailSubscriberService;
using MailBusinessLayer.Services.SubscriberPreferenceService;
using MailDataAccessLayer.Models;
using MailInfrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace MailBusinessLayer.DependencyInjection;

public static class BLDependencyInjection
{
    private static void RegisterMappers(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MailSubscriberProfile));
        services.AddAutoMapper(typeof(SubscriberPreferenceProfile));

        services.AddAutoMapper(typeof(QueryMappingProfile));
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IMailService, MailService>();

        services.AddScoped<IGenericService<MailSubscriber, Guid>, MailSubscriberService>();
        services
            .AddScoped<IGenericService<SubscriberPreference, long>,
                SubscriberPreferenceService>();
    }

    private static void RegisterFacades(IServiceCollection services)
    {
        services.AddScoped<IMailFacade, MailFacade>();
        services.AddScoped<IGenericFacade<MailSubscriber, Guid, IGenericService<MailSubscriber, Guid>, 
                CreateMailSubscriberDto, UpdateMailSubscriberDto, ViewMailSubscriberDto, ViewMailSubscriberDto>,
            MailSubscriberFacade>();
    }

    private static void RegisterQuartzJobs(IServiceCollection services)
    {
        services.AddQuartz(quartz =>
        {
            var jobKey = new JobKey("ARKKSendMailJob");
            quartz.AddJob<ARKKSendMailJob>(opts => opts.WithIdentity(jobKey));

            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("ARKKSendMailJob-trigger")
                .WithCronSchedule("0 0 12 ? * MON-FRI")
            );
        });
        services.AddQuartzHostedService(quartz => quartz.WaitForJobsToComplete = true);
    }
    
    public static void RegisterBLDependecies(this IServiceCollection services)
    {
        RegisterMappers(services);
        RegisterQuartzJobs(services);
        RegisterServices(services);
        RegisterFacades(services);
    }
}
