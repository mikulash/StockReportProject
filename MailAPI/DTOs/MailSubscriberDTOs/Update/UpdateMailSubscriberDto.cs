using System.ComponentModel.DataAnnotations;
using MailAPI.DTOs.SubscriberPreferenceDTOs.Create;

namespace MailAPI.DTOs.MailSubscriberDTOs.Update;

public class UpdateMailSubscriberDto
{
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public required string Email { get; set; }
    public required List<CreateSubscriberPreferenceDto> Preferences { get; set; }
}
