using GenericInfrastructure.Query;
using GenericInfrastructure.Repository;
using GenericInfrastructure.UnitOfWork;
using MailDataAccessLayer.Models;

namespace MailInfrastructure.UnitOfWork;

public interface IMailUnitOfWork : IBaseUnitOfWork
{
    IGenericRepository<MailSubscriber, Guid> MailSubscriberRepository { get; init; }
    IQuery<MailSubscriber, Guid> MailSubscriberQuery { get; init; }
    IGenericRepository<SubscriberPreference, long> SubscriberPreferenceRepository { get; init; }
    IQuery<SubscriberPreference, long> SubscriberPreferenceQuery { get; init; }
}
