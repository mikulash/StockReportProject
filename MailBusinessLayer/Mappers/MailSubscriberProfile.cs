using AutoMapper;
using MailAPI.DTOs.MailSubscriberDTOs.Create;
using MailAPI.DTOs.MailSubscriberDTOs.Filter;
using MailAPI.DTOs.MailSubscriberDTOs.Update;
using MailAPI.DTOs.MailSubscriberDTOs.View;
using MailDataAccessLayer.Models;
using MailInfrastructure.EntityFilters;

namespace MailBusinessLayer.Mappers;

public class MailSubscriberProfile : Profile
{
    public MailSubscriberProfile()
    {
        CreateMap<CreateMailSubscriberDto, MailSubscriber>();
        CreateMap<UpdateMailSubscriberDto, MailSubscriber>();
        CreateMap<MailSubscriber, ViewMailSubscriberDto>();
        
        CreateMap<MailSubscriberFilterDto, MailSubscriberFilter>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
