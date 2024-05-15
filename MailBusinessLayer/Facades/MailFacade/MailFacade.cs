using GenericBusinessLayer.Services;
using MailBusinessLayer.Services;
using MailDataAccessLayer.Models;
using MailInfrastructure.EntityFilters;
using MailInfrastructure.UnitOfWork;

namespace MailBusinessLayer.Facades.MailFacade;

public class MailFacade: IMailFacade
{
    private readonly IMailService _mailService;
    private readonly IGenericService<SubscriberPreference, long> _subscriberPreferenceService;
    public MailFacade(IMailService service, IGenericService<SubscriberPreference, long>  subscriberPreferenceService)
    {
        _mailService = service;
        _subscriberPreferenceService = subscriberPreferenceService;
    }
    public async Task Send(string fundName, DateOnly date)
    {
        var preferences = await _subscriberPreferenceService.FetchFilteredAsync(new SubscriberPreferenceFilter(){ EQ_FundName = fundName},null);
        await _mailService.SendEmail(preferences.Items.ToList(),date);
    }
}