using GenericInfrastructure.Query;
using GenericInfrastructure.Repository;
using GenericInfrastructure.UnitOfWork;
using MailDataAccessLayer.Data;
using MailDataAccessLayer.Models;

namespace MailInfrastructure.UnitOfWork;

public class MailUnitOfWork : BaseUnitOfWork<MailDbContext>, IMailUnitOfWork
{
    public MailUnitOfWork(MailDbContext dbContext, 
        IGenericRepository<MailSubscriber, Guid> mailSubscriberRepository,
        IQuery<MailSubscriber, Guid> mailSubscriberQuery, 
        IGenericRepository<SubscriberPreference, long> subscriberPreferenceRepository, 
        IQuery<SubscriberPreference, long> subscriberPreferenceQuery) 
        : base(dbContext)
    {
        MailSubscriberRepository = mailSubscriberRepository;
        MailSubscriberQuery = mailSubscriberQuery;
        SubscriberPreferenceRepository = subscriberPreferenceRepository;
        SubscriberPreferenceQuery = subscriberPreferenceQuery;
    }

    public IGenericRepository<MailSubscriber, Guid> MailSubscriberRepository { get; init; }
    public IQuery<MailSubscriber, Guid> MailSubscriberQuery { get; init; }
    public IGenericRepository<SubscriberPreference, long> SubscriberPreferenceRepository { get; init; }
    public IQuery<SubscriberPreference, long> SubscriberPreferenceQuery { get; init; }
}