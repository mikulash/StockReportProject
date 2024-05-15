namespace MailAPI.DTOs.SubscriberPreferenceDTOs.View;

public class ViewSubscriberPreferenceDto
{
    public long Id { get; set; }
    public string FundName { get; set; } = string.Empty;
    public required string OutputType { get; set; }
}
