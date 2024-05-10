using GenericBusinessLayer.DTOs.BaseFilter;

namespace MailAPI.DTOs.MailSubscriberDTOs.Filter;

public class MailSubscriberFilterDto : FilterDto
{
    public string? CONTAINS_Email { get; set; }
}
