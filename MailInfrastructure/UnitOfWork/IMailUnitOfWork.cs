using GenericInfrastructure.Query;
using GenericInfrastructure.Repository;
using GenericInfrastructure.UnitOfWork;
using MailDataAccessLayer.Models;

namespace MailInfrastructure.UnitOfWork;

public interface IMailUnitOfWork : IBaseUnitOfWork
{
    IGenericRepository<MailSubscriber, long> MailSubscriberRepository { get; init; }
    //IQuery<MailSubscriber, long> MailSubscriberQuery { get; init; }
}
