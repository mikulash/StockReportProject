using GenericBusinessLayer.Services;
using GenericInfrastructure.Query;
using GenericInfrastructure.Query.Filters;
using MailDataAccessLayer.Models;
using MailInfrastructure.UnitOfWork;

namespace MailBusinessLayer.Services.SubscriberPreferenceService;

public class SubscriberPreferenceService: GenericService<SubscriberPreference,long,IMailUnitOfWork>
{
    public SubscriberPreferenceService(IMailUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<QueryResult<SubscriberPreference>> FetchFilteredAsync(IFilter<SubscriberPreference> filter, QueryParams? queryParams)
    {
        return await ExecuteQueryAsync(filter, queryParams, pref => pref.MailSubscriber);
    }
}