using System.ComponentModel.DataAnnotations;

namespace MailAPI.DTOs.MailSubscriberDTOs.Create;

public class CreateMailSubscriberDto
{
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public required string Email { get; set; }
}
