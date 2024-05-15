using GenericBusinessLayer.Exceptions;
using GenericBusinessLayer.Services;
using GenericInfrastructure.Query;
using GenericInfrastructure.Query.Filters;
using MailDataAccessLayer.Models;
using MailInfrastructure.UnitOfWork;

namespace MailBusinessLayer.Services.MailSubscriberService;

public class MailSubscriberService : GenericService<MailSubscriber, Guid, IMailUnitOfWork>
{
    public MailSubscriberService(IMailUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<IEnumerable<MailSubscriber>> FetchAllAsync() => 
        await Repository.GetAllAsync(null, sub => sub.Preferences!);

    public override async Task<MailSubscriber> FindByIdAsync(Guid id)
    {
        var entity = await Repository.GetByIdAsync(id, sub => sub.Preferences!);
        if (entity is null)
        {
            throw new NoSuchEntityException<Guid>(typeof(MailSubscriber), id);
        }
        return entity;
    }

    public override async Task<QueryResult<MailSubscriber>> FetchFilteredAsync(IFilter<MailSubscriber> filter, QueryParams? queryParams) 
        => await ExecuteQueryAsync(filter, queryParams, sub => sub.Preferences!);
}
