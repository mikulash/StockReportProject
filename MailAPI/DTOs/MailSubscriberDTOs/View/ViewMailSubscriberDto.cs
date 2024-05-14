using MailAPI.DTOs.SubscriberPreferenceDTOs.View;

namespace MailAPI.DTOs.MailSubscriberDTOs.View;

public class ViewMailSubscriberDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public required List<ViewSubscriberPreferenceDto> Preferences { get; set; }
}
