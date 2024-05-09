using GenericInfrastructure.Query;
using GenericInfrastructure.Repository;
using GenericInfrastructure.UnitOfWork;
using MailDataAccessLayer.Data;
using MailDataAccessLayer.Models;

namespace MailInfrastructure.UnitOfWork;

public class MailUnitOfWork : BaseUnitOfWork<MailDbContext>, IMailUnitOfWork
{
    public MailUnitOfWork(MailDbContext dbContext, 
        IGenericRepository<MailSubscriber, long> mailSubscriberRepository)
        //<MailSubscriber, long> mailSubscriberQuery) 
        : base(dbContext)
    {
        MailSubscriberRepository = mailSubscriberRepository;
        //MailSubscriberQuery = mailSubscriberQuery;
    }

    public IGenericRepository<MailSubscriber, long> MailSubscriberRepository { get; init; }
    //public IQuery<MailSubscriber, long> MailSubscriberQuery { get; init; }
}