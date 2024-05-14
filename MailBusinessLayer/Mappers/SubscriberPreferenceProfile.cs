using AutoMapper;
using MailAPI.DTOs.SubscriberPreferenceDTOs.Create;
using MailAPI.DTOs.SubscriberPreferenceDTOs.View;
using MailDataAccessLayer.Enums;
using MailDataAccessLayer.Models;

namespace MailBusinessLayer.Mappers;

public class SubscriberPreferenceProfile : Profile
{
    public SubscriberPreferenceProfile()
    {
        CreateMap<CreateSubscriberPreferenceDto, SubscriberPreference>()
            .ForMember(dest => dest.OutputType, 
                opt => opt.MapFrom(source => Enum.Parse<OutputType>(source.OutputType)));
        CreateMap<SubscriberPreference, ViewSubscriberPreferenceDto>()
            .ForMember(x => x.OutputType,
                opt => opt.MapFrom(source => source.OutputType.ToString()));
    }
}
