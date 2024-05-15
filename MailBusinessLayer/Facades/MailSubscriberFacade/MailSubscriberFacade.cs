using AutoMapper;
using GenericBusinessLayer.Facades;
using GenericBusinessLayer.Services;
using MailAPI.DTOs.MailSubscriberDTOs.Create;
using MailAPI.DTOs.MailSubscriberDTOs.Update;
using MailAPI.DTOs.MailSubscriberDTOs.View;
using MailDataAccessLayer.Models;
using MailInfrastructure.EntityFilters;

namespace MailBusinessLayer.Facades.MailSubscriberFacade;

public class MailSubscriberFacade : GenericFacade<MailSubscriber, Guid, IGenericService<MailSubscriber, Guid>, 
    CreateMailSubscriberDto, UpdateMailSubscriberDto, ViewMailSubscriberDto, ViewMailSubscriberDto, MailSubscriberFilter>
{
    public MailSubscriberFacade(IGenericService<MailSubscriber, Guid> service, IMapper mapper) : base(service, mapper)
    {
    }

    public override async Task<ViewMailSubscriberDto> CreateAsync(CreateMailSubscriberDto create)
    {
        create.Preferences = create.Preferences
            .DistinctBy(pref => new { pref.FundName, pref.OutputType }).ToList();
        
        var mailSubscriber = await Service.CreateAsync(Mapper.Map<MailSubscriber>(create));

        return Mapper.Map<ViewMailSubscriberDto>(mailSubscriber);
    }

    private static List<SubscriberPreference> GetNewPreferences(List<SubscriberPreference> oldPreferences, 
        List<SubscriberPreference> newlyDeclaredPreferences)
    {
        var keptPreferences =
            oldPreferences
                .Where(pref =>
                    newlyDeclaredPreferences
                        .Exists(newPref =>
                            newPref.FundName.Equals(pref.FundName) && newPref.OutputType.Equals(pref.OutputType)
                        )
                )
                .ToList();

        var newPreferences = newlyDeclaredPreferences
            .Where(newPref =>
                !keptPreferences
                    .Exists(pref
                        => pref.FundName.Equals(newPref.FundName) && pref.OutputType.Equals(newPref.OutputType)
                    )
            )
            .ToList();
        
        keptPreferences.AddRange(newPreferences);
        
        return keptPreferences.DistinctBy(pref => new { pref.FundName, pref.OutputType }).ToList();
    }
        

    public override async Task<ViewMailSubscriberDto> UpdateAsync(Guid key, UpdateMailSubscriberDto update)
    {
        var subscriber = await Service.FindByIdAsync(key);
        subscriber.Email = update.Email;

        var updatedPreferences = 
            GetNewPreferences(subscriber.Preferences?.ToList() ?? [],
            Mapper.Map<List<SubscriberPreference>>(update.Preferences));
        
        
        updatedPreferences.ForEach(pref => pref.MailSubscriber = subscriber);
        subscriber.Preferences = updatedPreferences;

        return Mapper.Map<ViewMailSubscriberDto>(await Service.UpdateAsync(subscriber));
    }
}