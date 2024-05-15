using GenericInfrastructure.Query;
using GenericInfrastructure.Repository;
using MailDataAccessLayer.Data;
using MailDataAccessLayer.Models;
using MailInfrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace MailInfrastructure.DependencyInjection;

public static class InfrastructureDependencyInjection
{
    public static void RegisterInfrastructureDependencies(this IServiceCollection services)
    {
        services.AddScoped<IGenericRepository<MailSubscriber, Guid>, GenericRepository<MailSubscriber, Guid, MailDbContext>>();
        services.AddScoped<IQuery<MailSubscriber, Guid>, QueryBase<MailSubscriber, Guid>>();
        services.AddScoped<IGenericRepository<SubscriberPreference, long>, GenericRepository<SubscriberPreference, long, MailDbContext>>();
        services.AddScoped<IQuery<SubscriberPreference, long>, QueryBase<SubscriberPreference, long>>();
        services.AddScoped<IMailUnitOfWork, MailUnitOfWork>();
    }
}
