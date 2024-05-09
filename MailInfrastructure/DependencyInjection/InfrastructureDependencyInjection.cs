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
        services.AddScoped<IGenericRepository<MailSubscriber, long>, GenericRepository<MailSubscriber, long, MailDbContext>>();
        services.AddScoped<IQuery<MailSubscriber, long>, QueryBase<MailSubscriber, long, IMailUnitOfWork>>();
        services.AddScoped<IMailUnitOfWork, MailUnitOfWork>();
    }
}
