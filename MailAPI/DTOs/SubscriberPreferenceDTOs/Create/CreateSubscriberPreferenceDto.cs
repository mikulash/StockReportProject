namespace MailAPI.DTOs.SubscriberPreferenceDTOs.Create;

public class CreateSubscriberPreferenceDto
{
    public string FundName { get; set; } = string.Empty;
    public required string OutputType { get; set; }
}
